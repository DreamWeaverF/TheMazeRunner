using TheMazeRunner;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace TheMazeRunner
{
    [MessagePackObject]
    public abstract class AMessageRequest : IMessage
    {
        [Key(100)]
        public int RpcId { get; set; }
    }
}
