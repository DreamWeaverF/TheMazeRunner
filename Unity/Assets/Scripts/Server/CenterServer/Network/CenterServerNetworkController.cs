using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TheMazeRunner
{
    public class G2CNetworkController : MonoBehaviour
    {
        private TService service;

        private Dictionary<long, GateProxySession> gateProxySessions = new Dictionary<long, GateProxySession>();
        private Dictionary<long, LoginProxySession> loginProxySessions = new Dictionary<long, LoginProxySession>();
        private Dictionary<long, UserProxySession> userProxySessions = new Dictionary<long, UserProxySession>();
        private Dictionary<long, ChatProxySession> chatProxySessions = new Dictionary<long, ChatProxySession>();
        private Dictionary<long, MailProxySession> mailProxySessions = new Dictionary<long, MailProxySession>();
        private Dictionary<long, FightProxySession> fightProxySessions = new Dictionary<long, FightProxySession>();

        public void OnReadMemoryStream(long _channelId, MemoryStream _stream)
        {

        }
    }
}
