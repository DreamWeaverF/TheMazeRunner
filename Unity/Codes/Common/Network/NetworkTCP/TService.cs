using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace TheMazeRunner
{
    public class TService : AService
    {
		public HashSet<long> NeedStartSend = new HashSet<long>();

		private Socket acceptor;
        private readonly SocketAsyncEventArgs innArgs = new SocketAsyncEventArgs();

		private readonly Dictionary<long, TChannel> idChannels = new Dictionary<long, TChannel>();

        void Start()
        {
            switch (serviceType)
            {
				case SERVICE_TYPE.Listener:
					acceptor = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					acceptor.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
					innArgs.Completed += this.OnComplete;
					acceptor.Bind(hostAndPortSO.InnerIPEndPoint);
					acceptor.Listen(1000);
					AcceptAsync();
					break;
				case SERVICE_TYPE.Connect:
					TChannel _channel = new TChannel(connectChannelID, hostAndPortSO.OuterIPEndPoint);
					idChannels.Add(connectChannelID, _channel);
					break;
            }
        }

        void OnDestroy()
        {
            switch (serviceType)
            {
				case SERVICE_TYPE.Listener:
                    innArgs.Completed -= this.OnComplete;
                    acceptor.Close();
					break;
            }    
        }

        void Update()
		{
			foreach (long channelId in this.NeedStartSend)
			{
				if(idChannels.TryGetValue(channelId,out TChannel _channel))
                {
					_channel.Update();
                }
			}
			this.NeedStartSend.Clear();
		}

		private void OnComplete(object _sender, SocketAsyncEventArgs _e)
		{
			switch (_e.LastOperation)
			{
				case SocketAsyncOperation.Accept:
					SocketError _socketError = _e.SocketError;
					Socket _acceptSocket = _e.AcceptSocket;
					OnAcceptComplete(_socketError, _acceptSocket);
					break;
				default:
					throw new Exception($"socket error: {_e.LastOperation}");
			}
		}

		private void AcceptAsync()
        {
            this.innArgs.AcceptSocket = null;
            if (this.acceptor.AcceptAsync(this.innArgs))
            {
                return;
            }
            OnAcceptComplete(this.innArgs.SocketError, this.innArgs.AcceptSocket);
        }

		private void OnAcceptComplete(SocketError _socketError, Socket _acceptSocket)
		{
			if (this.acceptor == null)
			{
				return;
			}

			// 开始新的accept
			this.AcceptAsync();

			if (_socketError != SocketError.Success)
			{
				Log.Error($"accept error {_socketError}");
				return;
			}

			try
			{
				long id = this.CreateAcceptChannelId(0);
				TChannel channel = new TChannel(id, _acceptSocket);
				this.idChannels.Add(channel.Id, channel);
			}
			catch (Exception exception)
			{
				Log.Error(exception);
			}
		}
		//
		public void Remove(long _id)
        {
			if (this.idChannels.TryGetValue(_id, out TChannel _channel))
			{
				_channel.Dispose();
			}
			this.idChannels.Remove(_id);
		}
		
		public override void Send(MemoryStream _stream,long _id = 0)
        {
			try
			{
				if(!idChannels.TryGetValue(_id, out TChannel _channel))
                {
					return;
                }
				_channel.Send(_stream);
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

    }
}
