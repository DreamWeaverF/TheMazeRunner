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
    public abstract class AMessageResponse : IMessage
    {
        [Key(100)]
        public int RpcId { get; set; }
        [Key(101)]
        public int ErrorCode { get; set; }
        [Key(102)]
        public string Message { get; set; }
    }

    public abstract class AMessageResponseHandler<T1, T2> : AMessageResponseSO where T1 : AMessageRequest where T2 : AMessageResponse, new()
    {
        private UnityEvent<AMessageRequest> sendMessageEvent;
        private readonly Dictionary<int, ETTask<T2>> requestCallbacks = new Dictionary<int, ETTask<T2>>();

        private int rpcId;

        public async ETTask<T2> OnSendMessage(T1 _request)
        {
            _request.RpcId = rpcId++;
            sendMessageEvent.Invoke(_request);
            ETTask<T2> _task = ETTask<T2>.Create(true);
            requestCallbacks.Add(_request.RpcId,_task);
            return await _task;
        }

        public override void OnHandMessage(AMessageResponse _response)
        {
            OnHandMessage(_response as T2);
            if(!requestCallbacks.TryGetValue(_response.RpcId,out ETTask<T2> _value))
            {
                return;
            }
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

        public override void AddListener(UnityAction<AMessageRequest> _call)
        {
            if (sendMessageEvent == null)
            {
                sendMessageEvent = new UnityEvent<AMessageRequest>();
            }
            sendMessageEvent.AddListener(_call);
        }

        public override void RemoveListener(UnityAction<AMessageRequest> _call)
        {
            sendMessageEvent.RemoveListener(_call);
        }
    }

    public abstract class AMessageResponseSO : ScriptableObject
    {
        public abstract void OnHandMessage(AMessageResponse _response);
        public abstract Type GetRequestType();
        public abstract Type GetResponseType();
        public abstract void AddListener(UnityAction<AMessageRequest> _call);
        public abstract void RemoveListener(UnityAction<AMessageRequest> _call);
    }
}
