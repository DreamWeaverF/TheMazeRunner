using TheMazeRunner;
using MessagePack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using System.Threading.Tasks;

namespace TheMazeRunner
{

    public class MessageController : AMessageController
    {
        [SerializeField]
        private List<AMessageRequestSenderSO> messageRequestSenderSOs;
        [SerializeField]
        private List<AMessageRequestHandlerSO> messageRequestHandlerSOs;
        [SerializeField]
        private List<AMessageNoticeSenderSO> messageNoticeSenderSOs;
        [SerializeField]
        private List<AMessageNoticeHandlerSO> messageNoticeHandlerSOs;

        private Dictionary<Type, AMessageRequestHandlerSO> messageRequestHandlers = new Dictionary<Type, AMessageRequestHandlerSO>();

        private Dictionary<Type, AMessageNoticeHandlerSO> messageNoticeHandlers = new Dictionary<Type, AMessageNoticeHandlerSO>();

        private readonly Dictionary<int, ETTask<AMessageResponse>> requestCallbacks = new Dictionary<int, ETTask<AMessageResponse>>();

        private int rpcId;

        private TService tcpService = new TService();


        void Start()
        {
            //switch (networkType)
            //{
            //    case NETWORK_TYPE.LISTENER:
            //        tcpService.StartListener(ipAddressSO.TryGetIPEndPoint(networkType));
            //        break;
            //    case NETWORK_TYPE.CONNECT:
            //        tcpService.StartConnect(ipAddressSO.TryGetIPEndPoint(networkType));
            //        break;
            //}


            //switch (networkType)
            //{
            //    case NETWORK_TYPE.LISTENER:
            //        service.StartListener(serviceType,ipAddressSO.TryGetIPEndPoint(networkType));
            //        break;
            //    case NETWORK_TYPE.CONNECT:
            //        service.StartConnect(serviceType, ipAddressSO.TryGetIPEndPoint(networkType));
            //        break;
            //}
            //service.ReadCallback += OnReadMessage;
            //service.AcceptCallback += OnAccept;

            if (messageRequestSenderSOs.Count > 0)
            {
                for (int i = 0; i < messageRequestSenderSOs.Count; i++)
                {
                    messageRequestSenderSOs[i].Init(this);
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
                    messageNoticeSenderSOs[i].Init(this);
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
            messageRequestHandlers.Clear();
            messageNoticeHandlers.Clear();
        }

        // Update is called once per frame
        void Update()
        {
            //service?.Update();
        }

        private void OnAccept(long _channelId, IPEndPoint _ipEndPoint)
        {
            //Log.Debug($"ÍøÂçÍæ¼ÒÁ¬½Ó {_ipEndPoint}");
        }

        //public override async Task<T2> SendMessage<T1,T2>(T1 _request)
        //{
        //    _request.RpcId = rpcId++;
        //    ETTask<AMessageResponse> _task = ETTask<AMessageResponse>.Create(true);
        //    requestCallbacks.Add(_request.RpcId, _task);

        //    long _channelId = RandomHelper.RandInt64();
        //    MemoryStream _memoryStream = new MemoryStream();
        //    await MessagePackSerializer.SerializeAsync(_memoryStream, _request);
        //    _memoryStream.Seek(0, SeekOrigin.Begin);
        //    //service.SendStream(_channelId, 0, _memoryStream);

        //    return await _task as T2;
        //}

        //public override async Task SendMessage<T1>(T1 _notice)
        //{
        //    long _channelId = RandomHelper.RandInt64();
        //    MemoryStream _memoryStream = new MemoryStream();
        //    await MessagePackSerializer.SerializeAsync(_memoryStream, _notice);
        //    _memoryStream.Seek(0, SeekOrigin.Begin);
        //    //service.SendStream(_channelId, 0, _memoryStream);
        //}

        private void OnReadMessage(TChannel _channel,MemoryStream _stream)
        {
            _stream.Position = 0;
            object _message = MessagePackSerializer.Deserialize<IMessage>(_stream);
            switch (_message)
            {
                case AMessageRequest _request:
                    OnReadMessageRequest(_channel, _request);
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
        private async void OnReadMessageRequest(TChannel _channel, AMessageRequest _request)
        {
            if(!messageRequestHandlers.TryGetValue(_request.GetType(),out AMessageRequestHandlerSO _so))
            {
                return;
            }
            IMessage _response = await _so.OnHandMessage(_request);
            MemoryStream _memoryStream = new MemoryStream();
            await MessagePackSerializer.SerializeAsync(_memoryStream, _response);
            _memoryStream.Seek(0, SeekOrigin.Begin);
            //service.SendStream(_channledId, 0, _memoryStream);
        }

        private void OnReadMessageResponse(AMessageResponse _response)
        {
            if(!requestCallbacks.TryGetValue(_response.RpcId,out ETTask<AMessageResponse> _task))
            {
                return;
            }
            _task.SetResult(_response);
            requestCallbacks.Remove(_response.RpcId);
        }

        private void OnReadMessageNotice(AMessageNotice _notice)
        {
            if (!messageNoticeHandlers.TryGetValue(_notice.GetType(), out AMessageNoticeHandlerSO _so))
            {
                return;
            }
            _so.OnHandMessage(_notice);
        }

        //protected override void OnReadMemoryStream(long _id, MemoryStream _stream)
        //{
        //    throw new NotImplementedException();
        //}

        //protected override void OnError(long _id, int _e)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
