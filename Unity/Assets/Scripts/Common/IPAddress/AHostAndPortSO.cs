using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace TheMazeRunner
{
    [Serializable]
    public class HostAndPort
    {
        public string OuterHost;
        public string InnerHost;
        public int Port;
    }
    public abstract class AHostAndPortSO : ScriptableObject
    {
        [SerializeField]
        public HostAndPort DevAddress;
        [SerializeField]
        public HostAndPort QaAddress;
        [SerializeField]
        public HostAndPort ReleaseAddress;

        public IPEndPoint OuterIPEndPoint
        {
            get
            {
                IPEndPoint _point = null;
                //switch (Launch.AppEnvironment)
                //{
                //    case APP_ENVIRONMENT.Dev:
                //        _point = new IPEndPoint(IPAddress.Parse(DevAddress.OuterHost), DevAddress.Port + Launch.AppIndex);
                //        break;
                //    case APP_ENVIRONMENT.Qa:
                //        _point = new IPEndPoint(IPAddress.Parse(QaAddress.OuterHost), QaAddress.Port + Launch.AppIndex);
                //        break;
                //    case APP_ENVIRONMENT.Release:
                //        _point = new IPEndPoint(IPAddress.Parse(ReleaseAddress.OuterHost), ReleaseAddress.Port + Launch.AppIndex);
                //        break;
                //}
                return _point;
            }
        }

        public IPEndPoint InnerIPEndPoint
        {
            get
            {
                IPEndPoint _point = null;
                //switch (Launch.AppEnvironment)
                //{
                //    case APP_ENVIRONMENT.Dev:
                //        _point = new IPEndPoint(IPAddress.Parse(DevAddress.InnerHost), DevAddress.Port + Launch.AppIndex);
                //        break;
                //    case APP_ENVIRONMENT.Qa:
                //        _point = new IPEndPoint(IPAddress.Parse(QaAddress.InnerHost), QaAddress.Port + Launch.AppIndex);
                //        break;
                //    case APP_ENVIRONMENT.Release:
                //        _point = new IPEndPoint(IPAddress.Parse(ReleaseAddress.InnerHost), ReleaseAddress.Port + Launch.AppIndex);
                //        break;
                //}
                return _point;
            }
        }
    }
}
