using MessagePack;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace TheMazeRunner
{
    //[MessagePack.Union(0, typeof(MessageRequestLogin1))]
    //[Union()]
    public abstract class IMessageRequest
    {
        [Key(100)]
        public int RpcId { get; set; }
        [Key(1000)]
        public ushort Opcode { get; set; }
    }

    [MessagePackObject]
    public class MessageRequestLogin1 : IMessageRequest
    {
        [Key(1)]
        public string UserName { get; set; }
        [Key(2)]
        public string Password { get; set; }
    }


    [MessagePackObject]
    public abstract class AMessageResponse
    {
        [Key(100)]
        public int RpcId { get; set; }
        [Key(101)]
        int ErrorCode { get; set; }
        [Key(102)]
        string Message { get; set; }
        [Key(1000)]
        public ushort Opcode { get; set; }
    }


    public abstract class AMessageRequestSO<T1,T2> : ScriptableObject, IMessageRequestSO where T1 : IMessageRequest where T2 : IMessageRequest
    {
        private UnityEvent<MemoryStream> sendMessageEvent;

        public async void OnSendMessage(IMessageRequest _request)
        {
            MemoryStream _memoryStream = new MemoryStream();
            await MessagePackSerializer.SerializeAsync(GetRequestType(), _memoryStream, _request);
            sendMessageEvent.Invoke(_memoryStream);
        }
        public void OnHandMessage(AMessageResponse _response)
        {
            OnHandMessage(_response as T2);
        }

        public Type GetRequestType()
        {
            return typeof(T1);
        }

        public Type GetResponseType()
        {
            return typeof(T2);
        }
        public abstract void OnHandMessage(T2 _response);

        public void AddListener(UnityAction<MemoryStream> _call)
        {
            sendMessageEvent.AddListener(_call);
        }

        public void RemoveListener(UnityAction<MemoryStream> _call)
        {
            sendMessageEvent.RemoveListener(_call);
        }
    }

    public interface IMessageRequestSO
    {
        void OnSendMessage(IMessageRequest _request);

        void OnHandMessage(AMessageResponse _response);

        Type GetRequestType();

        Type GetResponseType();

        void AddListener(UnityAction<MemoryStream> _call);

        void RemoveListener(UnityAction<MemoryStream> _call);
    }
}
