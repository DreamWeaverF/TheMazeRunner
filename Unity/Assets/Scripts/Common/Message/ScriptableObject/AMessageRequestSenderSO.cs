using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TheMazeRunner
{
    public abstract class AMessageRequestSenderSO : ScriptableObject
    {
        private NetworkController networkController;

        public void Init(NetworkController _tcpClientController)
        {
            networkController = _tcpClientController;
        }

        public async Task<T2> SendMessage<T1, T2>(T1 _request) where T1 : AMessageRequest where T2 : AMessageResponse
        {
            return await networkController.SendMessage<T1, T2>(_request);
        }
    }
}
