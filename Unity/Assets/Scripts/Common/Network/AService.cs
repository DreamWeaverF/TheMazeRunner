using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Events;

namespace TheMazeRunner
{
    public enum SERVICE_TYPE
    {
        None,
        Listener,
        Connect,
    }

    public abstract class AService : MonoBehaviour
    {
        [SerializeField]
        protected AHostAndPortSO hostAndPortSO;
        [SerializeField]
        protected SERVICE_TYPE serviceType;

        protected readonly long connectChannelID = 0;

        protected UnityEvent<long, MemoryStream> readStreamEvents = new UnityEvent<long, MemoryStream>();

        // localConn·ÅÔÚµÍ32bit
        private long acceptIdGenerater = 1;
        public long CreateAcceptChannelId(uint localConn)
        {
            return (++this.acceptIdGenerater << 32) | localConn;
        }

        public void InvokeReadStream(long _id,MemoryStream _stream)
        {
            readStreamEvents.Invoke(_id, _stream);
        }
        
        public void AddListenerReadStream(UnityAction<long,MemoryStream> _call)
        {
            readStreamEvents.AddListener(_call);
        }
        public void RemoveListenerReadStream(UnityAction<long,MemoryStream> _call)
        {
            readStreamEvents.RemoveListener(_call);
        }

        public abstract void Send(MemoryStream _stream, long _id = 0);
    }
}
