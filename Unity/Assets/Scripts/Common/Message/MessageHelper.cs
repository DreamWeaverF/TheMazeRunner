using MessagePack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

namespace TheMazeRunner
{
    public static class MessageHelper
    {
        //public static async Task<T2> SendHttpMessage<T1,T2>(T1 _sender,string _address) where T1 : IMessageRequest where T2 : AMessageResponse, new()
        //{
        //    T2 _response = new T2();
        //    if (!_sender.CheckMessageSender())
        //    {
        //        _response.ErrorCode = 100;
        //        return _response;
        //    }
        //    try
        //    {
        //        byte[] _senderByte = MessagePackSerializer.Serialize(_sender);

        //        //for(int i = 0 ; i < _senderByte.Length; i++)
        //        //{
        //        //    Debug.Log($"Byte Index {i} Value {_senderByte[i]}");
        //        //}

        //        byte[] _tresd1 = BitConverter.GetBytes(_sender.MessageId);

        //        Array.Reverse(_senderByte);
        //        ushort opcode = BitConverter.ToUInt16(_senderByte, 0);
        //        Debug.Log($"Result{opcode}");

        //        Array.Reverse(_senderByte);

        //        T1 _handler = MessagePackSerializer.Deserialize<T1>(_senderByte);
        //        //IMessageSender _handler1 = MessagePackSerializer.Deserialize<IMessageSender>(_senderByte);


        //        //HttpWebRequest _requestWeb = WebRequest.CreateHttp(_address);
        //        //_requestWeb.Method = "POST";
        //        //_requestWeb.ContentType = string.Format("application/octet-stream");
        //        //_requestWeb.Timeout = 5000;
        //        //_requestWeb.ContentLength = _senderByte.Length;
        //        //Stream _requestStream = await _requestWeb.GetRequestStreamAsync();
        //        //await _requestStream.WriteAsync(_senderByte, 0, _senderByte.Length);
        //        //_requestStream.Close();
        //        //HttpWebResponse _responseWeb = await _requestWeb.GetResponseAsync() as HttpWebResponse;
        //        //Stream _responseStream = _responseWeb.GetResponseStream();
        //        //_response = MessagePackSerializer.Deserialize<T2>(_responseStream);
        //        //_response.ExecuteMessageHandler(_sender);
        //        //_responseStream.Close();
        //        //_responseWeb.Dispose();

        //        //byte[] _test = new byte[4] { _senderByte[3],_senderByte[2], _senderByte[1], _senderByte[0] };
        //        //Debug.Log($"Test {_test}");
        //        //int i = BitConverter.ToInt32(_test, 0);
        //        //Debug.Log($"i {i}");

        //        //byte[] _tresd1 = BitConverter.GetBytes(_sender.MessageId);
        //        //for (int i = 0; i < _tresd1.Length; i++)
        //        //{
        //        //    Debug.Log($"Byte Index {i} Value {_tresd1[i]}");
        //        //}

        //        //IMessageSender _handler = MessagePackSerializer.Deserialize<IMessageSender>(_senderByte);



        //        //T1 _test2 = MessagePackSerializer.Deserialize<T1>(_senderByte);

        //        //T1 _test3 = _handler as T1;

        //    }
        //    catch (Exception _e)
        //    {
        //        _response.ErrorCode = 1;
        //        _response.Message = _e.Message;
        //    }
        //    finally
        //    {
        //        _response.ExecuteMessageHandler(_sender);
        //    }
        //    return _response;
        //}
    }
}
