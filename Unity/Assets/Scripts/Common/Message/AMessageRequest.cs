using ET;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace TheMazeRunner
{
    [MessagePackObject]
    public abstract class AMessageRequest : IMessage
    {
        [Key(100)]
        public int RpcId { get; set; }
    }

    public abstract class AMessageRequestSenderSO : ScriptableObject
    {
        public DelegateSendMessageEvent SendMessageEvent;
        public abstract void OnHandMessage(AMessageResponse _response);
        public abstract Type GetRequestType();
        public abstract Type GetResponseType();
    }

    public abstract class AMessageRequestSender<T1,T2> : AMessageRequestSenderSO where T1 : AMessageRequest where T2 : AMessageResponse, new()
    {
        private readonly Dictionary<int, ETTask<T2>> requestCallbacks = new Dictionary<int, ETTask<T2>>();

        private int rpcId;

        public async ETTask<T2> OnSendMessage(T1 _request)
        {
            _request.RpcId = rpcId++;
            SendMessageEvent.Invoke(_request);
            ETTask<T2> _task = ETTask<T2>.Create(true);
            requestCallbacks.Add(_request.RpcId, _task);
            return await _task;
        }

        public override void OnHandMessage(AMessageResponse _response)
        {
            if (!requestCallbacks.TryGetValue(_response.RpcId, out ETTask<T2> _value))
            {
                return;
            }
            OnHandMessage(_response as T2);
            _value.SetResult(_response as T2);
            requestCallbacks.Remove(_response.RpcId);
        }

        protected abstract void OnHandMessage(T2 _response);

        public override Type GetRequestType()
        {
            return typeof(T1);
        }

        public override Type GetResponseType()
        {
            return typeof(T2);
        }
    }
    public abstract class AMessageRequestHandlerSO : ScriptableObject
    {
        public abstract Task<IMessage> OnHandMessage(AMessageRequest _request);
        public abstract Type GetRequestType();
        public abstract Type GetResponseType();

    }

    public abstract class AMessageRequestHandler<T1,T2> : AMessageRequestHandlerSO where T1 : AMessageRequest where T2 : AMessageResponse,new()
    {
        private T2 response;
        public override async Task<IMessage> OnHandMessage(AMessageRequest _request)
        {
            if (response == null)
            {
                response = new T2();
            }
            await OnHandMessage(_request as T1, response);
            return response;
        }

        public override Type GetRequestType()
        {
            return typeof(T1);
        }

        public override Type GetResponseType()
        {
            return typeof(T2);
        }

        protected abstract Task OnHandMessage(T1 _request, T2 _response);
    }
}
