using System.Net;
using System.Net.Sockets;

namespace TheMazeRunner
{
    public class TCPChannel
    {
		private Socket socket;
		private SocketAsyncEventArgs innArgs = new SocketAsyncEventArgs();
		private SocketAsyncEventArgs outArgs = new SocketAsyncEventArgs();

		private readonly CircularBuffer recvBuffer = new CircularBuffer();
		private readonly CircularBuffer sendBuffer = new CircularBuffer();

		private void OnComplete(object sender, SocketAsyncEventArgs e)
		{
            //switch (e.LastOperation)
            //{
            //	case SocketAsyncOperation.Connect:
            //		this.Service.SynchronizationContextSO.Post(() => OnConnectComplete(e));
            //		break;
            //	case SocketAsyncOperation.Receive:
            //		this.Service.SynchronizationContextSO.Post(() => OnRecvComplete(e));
            //		break;
            //	case SocketAsyncOperation.Send:
            //		this.Service.SynchronizationContextSO.Post(() => OnSendComplete(e));
            //		break;
            //	case SocketAsyncOperation.Disconnect:
            //		this.Service.SynchronizationContextSO.Post(() => OnDisconnectComplete(e));
            //		break;
            //	default:
            //		throw new Exception($"socket error: {e.LastOperation}");
            //}
        }

		public void StartConnect(IPEndPoint ipEndPoint)
        {
			this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			this.socket.NoDelay = true;
			//this.parser = new PacketParser(this.recvBuffer, this.Service);
			this.innArgs.Completed += this.OnComplete;
			this.outArgs.Completed += this.OnComplete;

			this.outArgs.RemoteEndPoint = ipEndPoint;

			if (this.socket.ConnectAsync(this.outArgs))
			{
				return;
			}

			OnConnectComplete(this.outArgs);
		}

		private void OnConnectComplete(SocketAsyncEventArgs _e)
        {

        }
	}
}
