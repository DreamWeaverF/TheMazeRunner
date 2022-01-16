using ET;
using MessagePack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

namespace TheMazeRunner
{
    public class NetworkController : MonoBehaviour
    {
        [SerializeField]
        private List<AMessageRequestSO> messageRequestSOs;
        [SerializeField]
        private List<AMessageResponseSO> messageResponseSOs;
        //[SerializeField]
        //private List<IMessageRequestSO> messageRequestSOs = new List<IMessageRequestSO>();
        // Start is called before the first frame update

        [SerializeField]
        private bool IsServer;

        private Dictionary<Type,AMessageRequestSO> messageRequestHandlers = new Dictionary<Type, AMessageRequestSO>();

        private Dictionary<Type, AMessageResponseSO> messageResponseHandlers = new Dictionary<Type, AMessageResponseSO>();

        private AService service;
        void Start()
        {
            if (IsServer)
            {
                service = new TService(ThreadSynchronizationContext.Instance, NetworkHelper.ToIPEndPoint("127.0.0.1", 10002), ServiceType.Outer);
            }
            else
            {
                service = new TService(ThreadSynchronizationContext.Instance, ServiceType.Outer);
                //service.GetOrCreate(1, NetworkHelper.ToIPEndPoint("127.0.0.1", 20001));
            }
            service.ReadCallback += OnReadMessage;
            service.AcceptCallback += OnAccept;

            if (messageRequestSOs.Count > 0)
            {
                for (int i = 0; i < messageRequestSOs.Count; i++)
                {
                    messageRequestSOs[i].AddListener(OnSendMessage);
                    messageRequestHandlers.Add(messageRequestSOs[i].GetRequestType(), messageRequestSOs[i]);
                }
            }

            if (messageResponseSOs.Count > 0)
            {
                for (int i = 0; i < messageResponseSOs.Count; i++)
                {
                    messageResponseSOs[i].AddListener(OnSendMessage);
                    messageResponseHandlers.Add(messageResponseSOs[i].GetResponseType(), messageResponseSOs[i]);
                }
            }
        }

        void OnDestroy()
        {
            for (int i = 0; i < messageRequestSOs.Count; i++)
            {
                messageRequestSOs[i].RemoveListener(OnSendMessage);
            }
            messageRequestHandlers.Clear();
        }

        // Update is called once per frame
        void Update()
        {
            service?.Update();
        }

        private void OnAccept(long _channelId, IPEndPoint _ipEndPoint)
        {
            //Log.Debug($"ÍøÂçÍæ¼ÒÁ¬½Ó {_ipEndPoint}");
        }

        private async void OnSendMessage(IMessage _rerquest)
        {
            IPEndPoint _realIPEndPoint = NetworkHelper.ToIPEndPoint("127.0.0.1", 10002);
            long _channelId = RandomHelper.RandInt64();
            service.GetOrCreate(_channelId, _realIPEndPoint);

            MemoryStream _memoryStream = new MemoryStream();
            await MessagePackSerializer.SerializeAsync(_memoryStream, _rerquest);
            _memoryStream.Seek(0, SeekOrigin.Begin);

            //object _message = MessagePackSerializer.Deserialize<IMessage>(_memoryStream);

            service.SendStream(_channelId, 0, _memoryStream);
        }

        private void OnReadMessage(long _channledId,MemoryStream _stream)
        {
            _stream.Position = 0;
            object _message = MessagePackSerializer.Deserialize<IMessage>(_stream);
            switch (_message)
            {
                case AMessageRequest _request:
                    OnReadMessageRequest(1, _request);
                    break;
                case AMessageResponse _response:

                    break;
                case AMessageNotice _notice:

                    break;
            }
        }
        //
        private async void OnReadMessageRequest(long _channledId, AMessageRequest _request)
        {
            if(!messageRequestHandlers.TryGetValue(_request.GetType(),out AMessageRequestSO _so))
            {
                return;
            }
            AMessageResponse _response = await _so.OnHandMessage(_request);
            MemoryStream _memoryStream = new MemoryStream();
            await MessagePackSerializer.SerializeAsync(_memoryStream, _response);
            //todolist sendMessage
        }
    }
}
