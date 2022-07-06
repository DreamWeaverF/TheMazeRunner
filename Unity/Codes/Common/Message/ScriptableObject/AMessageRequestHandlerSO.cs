using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace TheMazeRunner
{
    public abstract class AMessageRequestHandlerSO<T1,T2> : AMessageRequestHandlerSO where T1 : AMessageRequest where T2 : AMessageResponse, new()
    {
        protected T2 _response = new T2();

        public override async Task<AMessageResponse> OnHandMessage(AMessageRequest _notice)
        {
            _response.ErrorCode = 0;
            await OnHandMessage(_notice as T1);
            return _response;
        }

        public abstract Task OnHandMessage(T1 _request);

        public override Type GetRequestType()
        {
            return typeof(T1);
        }
    }

    public abstract class AMessageRequestHandlerSO : ScriptableObject
    {
        public abstract Task<AMessageResponse> OnHandMessage(AMessageRequest _notice);
        public abstract Type GetRequestType();
    }
}
