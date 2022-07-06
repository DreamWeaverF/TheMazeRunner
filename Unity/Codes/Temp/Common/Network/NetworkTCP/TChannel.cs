using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace TheMazeRunner
{
    public class TChannel : IDisposable
	{
        public long Id;
        public IPEndPoint RemoteAddress;
		public TService Service;

        private Socket socket;
		private SocketAsyncEventArgs innArgs = new SocketAsyncEventArgs();
		private SocketAsyncEventArgs outArgs = new SocketAsyncEventArgs();

		private readonly CircularBuffer recvBuffer = new CircularBuffer();
		private readonly CircularBuffer sendBuffer = new CircularBuffer();

		private bool isSending;
		private bool isConnected;

		private readonly PacketParser parser; 
		private readonly byte[] sendCache = new byte[sizeof(ushort)];

		public TChannel(long _id, Socket _socket)
        {
            Id = _id;
            socket = _socket;
            socket.NoDelay = true;
            this.parser = new PacketParser(this.recvBuffer);
            this.innArgs.Completed += this.OnComplete;
            this.outArgs.Completed += this.OnComplete;

            this.RemoteAddress = (IPEndPoint)socket.RemoteEndPoint;
            this.isConnected = true;
            this.isSending = false;

            this.StartRecv();
            this.StartSend();
        }

		public TChannel(long _id, IPEndPoint _ipEndPoint)
        {
			this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			this.socket.NoDelay = true;
            this.parser = new PacketParser(this.recvBuffer);
            this.innArgs.Completed += this.OnComplete;
			this.outArgs.Completed += this.OnComplete;

			this.outArgs.RemoteEndPoint = _ipEndPoint;

			if (this.socket.ConnectAsync(this.outArgs))
			{
				return;
			}

			OnConnectComplete(this.outArgs);
		}

		public bool IsDisposed
		{
			get
			{
				return this.Id == 0;
			}
		}

		public void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			Log.Info($"channel dispose: {this.Id} {this.RemoteAddress}");

			long id = this.Id;
			this.Id = 0;
			this.Service.Remove(id);
			this.socket.Close();
			this.innArgs.Dispose();
			this.outArgs.Dispose();
			this.innArgs = null;
			this.outArgs = null;
			this.socket = null;
		}

		private void OnComplete(object _sender, SocketAsyncEventArgs _e)
        {
            switch (_e.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    OnConnectComplete(_e);
                    break;
                case SocketAsyncOperation.Receive:
                    OnRecvComplete(_e);
                    break;
                case SocketAsyncOperation.Send:
                    OnSendComplete(_e);
                    break;
                case SocketAsyncOperation.Disconnect:
                    OnDisconnectComplete(_e);
                    break;
                default:
                    throw new Exception($"socket error: {_e.LastOperation}");
            }
        }



		public void Send(MemoryStream _stream)
		{
			if (this.IsDisposed)
			{
				throw new Exception("TChannel已经被Dispose, 不能发送消息");
			}
			ushort messageSize = (ushort)(_stream.Length - _stream.Position);

			this.sendCache.WriteTo(0, messageSize);
			this.sendBuffer.Write(this.sendCache, 0, PacketParser.OuterPacketSizeLength);

			this.sendBuffer.Write(_stream.GetBuffer(), (int)_stream.Position, (int)(_stream.Length - _stream.Position));

			if (!this.isSending)
			{
				//this.StartSend();
				this.Service.NeedStartSend.Add(this.Id);
			}
		}

		private void ConnectAsync()
		{
			this.outArgs.RemoteEndPoint = this.RemoteAddress;
			if (this.socket.ConnectAsync(this.outArgs))
			{
				return;
			}
			OnConnectComplete(this.outArgs);
		}

		private void OnConnectComplete(object o)
		{
			if (this.socket == null)
			{
				return;
			}
			SocketAsyncEventArgs e = (SocketAsyncEventArgs)o;

			if (e.SocketError != SocketError.Success)
			{
				this.OnError((int)e.SocketError);
				return;
			}

			e.RemoteEndPoint = null;
			this.isConnected = true;
			this.StartRecv();
			this.StartSend();
		}

		private void OnDisconnectComplete(object o)
		{
			SocketAsyncEventArgs e = (SocketAsyncEventArgs)o;
			this.OnError((int)e.SocketError);
		}

		private void StartRecv()
		{
			while (true)
			{
				try
				{
					if (this.socket == null)
					{
						return;
					}

					int size = this.recvBuffer.ChunkSize - this.recvBuffer.LastIndex;
					this.innArgs.SetBuffer(this.recvBuffer.Last, this.recvBuffer.LastIndex, size);
				}
				catch (Exception e)
				{
					Log.Error($"tchannel error: {this.Id}\n{e}");
					this.OnError(ErrorCore.ERR_TChannelRecvError);
					return;
				}

				if (this.socket.ReceiveAsync(this.innArgs))
				{
					return;
				}
				this.HandleRecv(this.innArgs);
			}
		}

		private void OnRecvComplete(object o)
		{
			this.HandleRecv(o);

			if (this.socket == null)
			{
				return;
			}
			this.StartRecv();
		}

		private void HandleRecv(object o)
		{
			if (this.socket == null)
			{
				return;
			}
			SocketAsyncEventArgs e = (SocketAsyncEventArgs)o;

			if (e.SocketError != SocketError.Success)
			{
				this.OnError((int)e.SocketError);
				return;
			}

			if (e.BytesTransferred == 0)
			{
				this.OnError(ErrorCore.ERR_PeerDisconnect);
				return;
			}

			this.recvBuffer.LastIndex += e.BytesTransferred;
			if (this.recvBuffer.LastIndex == this.recvBuffer.ChunkSize)
			{
				this.recvBuffer.AddLast();
				this.recvBuffer.LastIndex = 0;
			}

			// 收到消息回调
			while (true)
			{
				// 这里循环解析消息执行，有可能，执行消息的过程中断开了session
				if (this.socket == null)
				{
					return;
				}
				try
				{
					bool ret = this.parser.Parse();
					if (!ret)
					{
						break;
					}

					this.OnRead(this.parser.MemoryStream);
				}
				catch (Exception ee)
				{
					Log.Error($"ip: {this.RemoteAddress} {ee}");
					this.OnError(ErrorCore.ERR_SocketError);
					return;
				}
			}
		}

		public void Update()
		{
			this.StartSend();
		}

		private void StartSend()
		{
			if (!this.isConnected)
			{
				return;
			}

			if (this.isSending)
			{
				return;
			}

			while (true)
			{
				try
				{
					if (this.socket == null)
					{
						this.isSending = false;
						return;
					}

					// 没有数据需要发送
					if (this.sendBuffer.Length == 0)
					{
						this.isSending = false;
						return;
					}

					this.isSending = true;

					int sendSize = this.sendBuffer.ChunkSize - this.sendBuffer.FirstIndex;
					if (sendSize > this.sendBuffer.Length)
					{
						sendSize = (int)this.sendBuffer.Length;
					}

					this.outArgs.SetBuffer(this.sendBuffer.First, this.sendBuffer.FirstIndex, sendSize);

					if (this.socket.SendAsync(this.outArgs))
					{
						return;
					}

					HandleSend(this.outArgs);
				}
				catch (Exception e)
				{
					throw new Exception($"socket set buffer error: {this.sendBuffer.First.Length}, {this.sendBuffer.FirstIndex}", e);
				}
			}
		}

		private void OnSendComplete(object o)
		{
			HandleSend(o);

			this.isSending = false;

			this.StartSend();
		}

		private void HandleSend(object o)
		{
			if (this.socket == null)
			{
				return;
			}

			SocketAsyncEventArgs e = (SocketAsyncEventArgs)o;

			if (e.SocketError != SocketError.Success)
			{
				this.OnError((int)e.SocketError);
				return;
			}

			if (e.BytesTransferred == 0)
			{
				this.OnError(ErrorCore.ERR_PeerDisconnect);
				return;
			}

			this.sendBuffer.FirstIndex += e.BytesTransferred;
			if (this.sendBuffer.FirstIndex == this.sendBuffer.ChunkSize)
			{
				this.sendBuffer.FirstIndex = 0;
				this.sendBuffer.RemoveFirst();
			}
		}

		private void OnRead(MemoryStream _memoryStream)
		{
			try
			{
				this.Service.InvokeReadStream(this.Id, _memoryStream);
			}
			catch (Exception e)
			{
				Log.Error($"{this.RemoteAddress} {_memoryStream.Length} {e}");
				// 出现任何消息解析异常都要断开Session，防止客户端伪造消息
				this.OnError(ErrorCore.ERR_PacketParserError);
			}
		}

		private void OnError(int error)
		{
			Log.Info($"TChannel OnError: {error} {this.RemoteAddress}");
			long channelId = this.Id;
		}
	}
}
