using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazeRunner
{
    public abstract class AMessageNoticeSenderSO : ScriptableObject
    {
        private NetworkController networkController;

        public void Init(NetworkController _networkController)
        {
            networkController = _networkController;
        }

        public async void SendMessage<T1>(T1 _notice) where T1 : AMessageNotice
        {
            await networkController.SendMessage(_notice);
        }
    }
}
