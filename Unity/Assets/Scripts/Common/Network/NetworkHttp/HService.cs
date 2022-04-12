using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using UnityEngine;

namespace TheMazeRunner
{
    public class HService : AService
    {
        private HttpListener listener;
        private HttpClient client;
        // Start is called before the first frame update
        void Start()
        {
            switch (serviceType)
            {
                case SERVICE_TYPE.Listener:
                    listener = new HttpListener();
                    listener.Prefixes.Add("http://127.0.0.1:80/");
                    listener.Start();
                    listener.BeginGetContext(new AsyncCallback(GetContextCallback), listener);
                    break;
                case SERVICE_TYPE.Connect:
                    client = new HttpClient();
                    break;
            }
        }

        void OnDestroy()
        {
            if(listener != null)
            {
                listener.Stop();
            }   
            if(client != null)
            {
                client.Dispose();
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public override async void Send(MemoryStream _stream, long _id = 0)
        {
            if(client == null)
            {
                return;
            }
            HttpContent _content = new StreamContent(_stream);
            HttpResponseMessage _response = await client.PostAsync("http://127.0.0.1:80/", _content);
            if (_response.StatusCode != HttpStatusCode.OK)
            {
                return;

            }
            Stream _outStream = await _response.Content.ReadAsStreamAsync();
            InvokeReadStream(_id, _outStream as MemoryStream);
        }

        private void GetContextCallback(IAsyncResult _result)
        {
            listener = _result.AsyncState as HttpListener;
            HttpListenerContext _context = listener.EndGetContext(_result);
            listener.BeginGetContext(new AsyncCallback(GetContextCallback), listener);
            HttpListenerRequest _request = _context.Request;
            HttpListenerResponse _response = _context.Response;
            //MemoryStream _stream = _request.InputStream as MemoryStream;
            //MemoryStream _stream = new MemoryStream();
            //_stream.SetLength(sizeof(long));
            //_stream.GetBuffer().WriteTo(0, _testL);
            _response.OutputStream.Write(new byte[8],0,8);
            _response.OutputStream.Close();
        }
    }
}
