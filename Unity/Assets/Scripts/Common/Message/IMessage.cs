using MessagePack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazeRunner
{
    [Union(0, typeof(MessageRequestLogin))]
    [Union(1, typeof(MessageResponseLogin))]

    [Union(10, typeof(MessageResponseRegister))]
    [Union(11, typeof(MessageRequestRegister))]
    public interface IMessage 
    {
        
    }
}
