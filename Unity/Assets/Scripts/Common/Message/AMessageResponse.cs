using ET;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace TheMazeRunner
{
    [MessagePackObject]
    public abstract class AMessageResponse : IMessage
    {
        [Key(100)]
        public int RpcId { get; set; }
        [Key(101)]
        public int ErrorCode { get; set; }
        [Key(102)]
        public string Message { get; set; }
    }
}
