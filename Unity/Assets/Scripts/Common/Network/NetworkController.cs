using MessagePack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TheMazeRunner
{
    public class NetworkController : MonoBehaviour
    {
        [SerializeField]
        private List<IMessageRequestSO> messageRequestSOs = new List<IMessageRequestSO>();
        // Start is called before the first frame update

        private Dictionary<Type,IMessageRequestSO> messageRequestSODict = new Dictionary<Type, IMessageRequestSO>();
        void Start()
        {
            //for(int i = 0; i < messageRequestSOs.Count; i++)
            //{
            //    messageRequestSOs[i].AddListener(OnSendMessage);
            //    messageRequestSODict.Add(messageRequestSOs[i].GetRequestType(), messageRequestSOs[i]);
            //}

            IMessageRequest data = new MessageRequestLogin1() { UserName = "xxx" };
            // serialize interface.
            var bin = MessagePackSerializer.Serialize(data);
            // deserialize interface.
            var reData = MessagePackSerializer.Deserialize<IMessageRequest>(bin);

            switch (reData)
            {
                case MessageRequestLogin1 login1:

                    break;
            }

        }

        void OnDestroy()
        {
            for (int i = 0; i < messageRequestSOs.Count; i++)
            {
                messageRequestSOs[i].RemoveListener(OnSendMessage);
            }
            messageRequestSODict.Clear();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnSendMessage(MemoryStream _stream)
        {
            OnReadMessage(_stream);
        }

        private void OnReadMessage(MemoryStream _stream)
        {
            //object _message = MessagePackSerializer.Deserialize(_stream);
        }
    }
}
