using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace TheMazeRunner
{
    public class Client2GateMessageController : AMessageController
    {
        [SerializeField]
        private Gate2LoginMessageController g2lMessageController;
        [SerializeField]
        private Gate2CenterMessageController g2cMessageController;
        //
        private Dictionary<long,long> channleId2UserIds = new Dictionary<long,long>();
        //
        private Dictionary<long, long> userId2ChannleIds = new Dictionary<long, long>();

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

