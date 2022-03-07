using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace TheMazeRunner
{
    public class TCPService
    {
        private Socket acceptor;
        private readonly SocketAsyncEventArgs innArgs = new SocketAsyncEventArgs();

		private TCPChannel channel = new TCPChannel();

		public void StartListener(IPEndPoint ipEndPoint)
        {
			acceptor = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			acceptor.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			innArgs.Completed += this.OnComplete;
			acceptor.Bind(ipEndPoint);
			acceptor.Listen(1000);
			AcceptAsync();
		}

		public void StartConnect(IPEndPoint ipEndPoint)
        {
			channel.StartConnect(ipEndPoint);

		}

		private void OnComplete(object sender, SocketAsyncEventArgs e)
		{
			switch (e.LastOperation)
			{
				case SocketAsyncOperation.Accept:
					SocketError socketError = e.SocketError;
					Socket acceptSocket = e.AcceptSocket;
					OnAcceptComplete(socketError, acceptSocket);
					break;
				default:
					throw new Exception($"socket error: {e.LastOperation}");
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

		private void OnAcceptComplete(SocketError socketError, Socket acceptSocket)
		{
			if (this.acceptor == null)
			{
				return;
			}

			// 开始新的accept
			this.AcceptAsync();

			if (socketError != SocketError.Success)
			{
				Log.Error($"accept error {socketError}");
				return;
			}

			try
			{
				long id = 0;
				//TChannel channel = new TChannel(id, acceptSocket, this);
				//this.idChannels.Add(channel.Id, channel);
				//long channelId = channel.Id;

				//this.OnAccept(channelId, channel.RemoteAddress);
			}
			catch (Exception exception)
			{
				Log.Error(exception);
			}
		}
	}
}
