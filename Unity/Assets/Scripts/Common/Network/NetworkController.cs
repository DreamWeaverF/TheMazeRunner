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
        private List<AMessageRequestSenderSO> messageRequestSenderSOs;
        [SerializeField]
        private List<AMessageRequestHandlerSO> messageRequestHandlerSOs;

        [SerializeField]
        private List<AMessageNoticeSenderSO> messageNoticeSenderSOs;
        [SerializeField]
        private List<AMessageNoticeHandlerSO> messageNoticeHandlerSOs;

        [SerializeField]
        private bool IsServer;

        private Dictionary<Type, AMessageRequestSenderSO> messageRequestSenders = new Dictionary<Type, AMessageRequestSenderSO>();

        private Dictionary<Type, AMessageRequestHandlerSO> messageRequestHandlers = new Dictionary<Type, AMessageRequestHandlerSO>();

        private Dictionary<Type, AMessageNoticeSenderSO> messageNoticeSenders = new Dictionary<Type, AMessageNoticeSenderSO>();

        private Dictionary<Type, AMessageNoticeHandlerSO> messageNoticeHandlers = new Dictionary<Type, AMessageNoticeHandlerSO>();


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

            if (messageRequestSenderSOs.Count > 0)
            {
                for (int i = 0; i < messageRequestSenderSOs.Count; i++)
                {
                    messageRequestSenderSOs[i].SendMessageEvent += OnSendMessage;
                    messageRequestSenders.Add(messageRequestSenderSOs[i].GetResponseType(), messageRequestSenderSOs[i]);
                }
            }

            if (messageRequestHandlerSOs.Count > 0)
            {
                for (int i = 0; i < messageRequestHandlerSOs.Count; i++)
                {
                    messageRequestHandlers.Add(messageRequestHandlerSOs[i].GetRequestType(), messageRequestHandlerSOs[i]);
                }
            }

            if (messageNoticeSenderSOs.Count > 0)
            {
                for (int i = 0; i < messageNoticeSenderSOs.Count; i++)
                {
                    messageNoticeSenderSOs[i].SendMessageEvent += OnSendMessage;
                    messageNoticeSenders.Add(messageNoticeSenderSOs[i].GetNoticeType(), messageNoticeSenderSOs[i]);
                }
            }

            if (messageNoticeHandlerSOs.Count > 0)
            {
                for (int i = 0; i < messageNoticeHandlerSOs.Count; i++)
                {
                    messageNoticeHandlers.Add(messageNoticeHandlerSOs[i].GetNoticeType(), messageNoticeHandlerSOs[i]);
                }
            }
        }

        void OnDestroy()
        {
            for (int i = 0; i < messageRequestSenderSOs.Count; i++)
            {
                messageRequestSenderSOs[i].SendMessageEvent -= OnSendMessage;
            }
            messageRequestSenderSOs.Clear();
            messageRequestHandlers.Clear();
            for (int i = 0; i < messageNoticeSenderSOs.Count; i++)
            {
                messageNoticeSenderSOs[i].SendMessageEvent -= OnSendMessage;
            }
            messageNoticeSenders.Clear();
            messageNoticeHandlers.Clear();
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

        private async void OnSendMessage(IMessage _message)
        {
            IPEndPoint _realIPEndPoint = NetworkHelper.ToIPEndPoint("127.0.0.1", 10002);
            long _channelId = RandomHelper.RandInt64();
            service.GetOrCreate(_channelId, _realIPEndPoint);

            MemoryStream _memoryStream = new MemoryStream();
            await MessagePackSerializer.SerializeAsync(_memoryStream, _message);
            _memoryStream.Seek(0, SeekOrigin.Begin);
            service.SendStream(_channelId, 0, _memoryStream);
        }

        private void OnReadMessage(long _channledId,MemoryStream _stream)
        {
            _stream.Position = 0;
            object _message = MessagePackSerializer.Deserialize<IMessage>(_stream);
            switch (_message)
            {
                case AMessageRequest _request:
                    OnReadMessageRequest(_channledId, _request);
                    break;
                case AMessageResponse _response:
                    OnReadMessageResponse(_response);
                    break;
                case AMessageNotice _notice:
                    OnReadMessageNotice(_notice);
                    break;
            }
        }
        //
        private async void OnReadMessageRequest(long _channledId, AMessageRequest _request)
        {
            if(!messageRequestHandlers.TryGetValue(_request.GetType(),out AMessageRequestHandlerSO _so))
            {
                return;
            }
            IMessage _response = await _so.OnHandMessage(_request);
            MemoryStream _memoryStream = new MemoryStream();
            await MessagePackSerializer.SerializeAsync(_memoryStream, _response);
            _memoryStream.Seek(0, SeekOrigin.Begin);
            service.SendStream(_channledId, 0, _memoryStream);
        }

        private void OnReadMessageResponse(AMessageResponse _response)
        {
            if (!messageRequestSenders.TryGetValue(_response.GetType(), out AMessageRequestSenderSO _so))
            {
                return;
            }
            _so.OnHandMessage(_response);
        }

        private void OnReadMessageNotice(AMessageNotice _notice)
        {
            if (!messageNoticeHandlers.TryGetValue(_notice.GetType(), out AMessageNoticeHandlerSO _so))
            {
                return;
            }
            _so.OnHandMessage(_notice);
        }
    }
}
