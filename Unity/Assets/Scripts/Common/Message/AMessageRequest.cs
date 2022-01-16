using MessagePack;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace TheMazeRunner
{
    [MessagePackObject]
    public class AMessageRequest : IMessage
    {
        [Key(100)]
        public int RpcId { get; set; }
    }

    public abstract class AMessageRequestHandler<T1,T2> : AMessageRequestSO where T1 : AMessageRequest where T2 : AMessageResponse, new()
    {
        private UnityEvent<AMessageResponse> sendMessageEvent;
        private T2 response;

        public override async Task<AMessageResponse> OnHandMessage(AMessageRequest _request)
        {
            if (response == null)
            {
                response = new T2();
            }
            await OnHandMessage(_request as T1, response);
            return response;
        }

        protected abstract Task OnHandMessage(T1 _request, T2 _response);

        public override Type GetRequestType()
        {
            return typeof(T1);
        }

        public override Type GetResponseType()
        {
            return typeof(T2);
        }

        public override void AddListener(UnityAction<AMessageResponse> _call)
        {
            if(sendMessageEvent == null)
            {
                sendMessageEvent = new UnityEvent<AMessageResponse>();
            }
            sendMessageEvent.AddListener(_call);
        }

        public override void RemoveListener(UnityAction<AMessageResponse> _call)
        {
            sendMessageEvent.RemoveListener(_call);
        }
    }

    public abstract class AMessageRequestSO : ScriptableObject
    {
        public abstract Task<AMessageResponse> OnHandMessage(AMessageRequest _request);
        public abstract Type GetRequestType();
        public abstract Type GetResponseType();
        public abstract void AddListener(UnityAction<AMessageResponse> _call);
        public abstract void RemoveListener(UnityAction<AMessageResponse> _call);
    }
}
