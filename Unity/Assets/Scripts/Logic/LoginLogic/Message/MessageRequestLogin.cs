using MessagePack;
using UnityEngine;

namespace TheMazeRunner
{
    [MessagePackObject]
    public class MessageRequestLogin : IMessageRequest
    {
        [Key(1)]
        public string UserName { get; set; }
        [Key(2)]
        public string Password { get; set; }
    }

    [MessagePackObject]
    public class MessageResponseLogin : AMessageResponse
    {
        private IMessageRequestSO messageRequestSO;
    }

    public class MessageRequestLoginSO : ScriptableObject
    {

    }
}
