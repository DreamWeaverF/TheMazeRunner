using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace TheMazeRunner
{
    public abstract class AMessageController : MonoBehaviour
    {
        //[SerializeField]
        //protected AHostAndPortSO ipAddressSO;
        //[SerializeField]
        //protected AService serviceSO;
        //public abstract Task<T2> SendMessage<T1, T2>(T1 _request) where T1 : AMessageRequest where T2 : AMessageResponse;
        //public abstract Task SendMessage<T1>(T1 _notice) where T1 : AMessageNotice;
        //protected abstract void OnReadMemoryStream(long _id, MemoryStream _stream);
        //protected abstract void OnError(long _id, int _e);

        //void Start()
        //{
        //    serviceSO?.AddListenerReadStream(OnReadMemoryStream);
        //}

        //private void OnDestroy()
        //{
        //    serviceSO?.RemoveListenerReadStream(OnReadMemoryStream);
        //}
    }
}
