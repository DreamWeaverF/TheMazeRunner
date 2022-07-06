using MessagePack;

namespace TheMazeRunner
{
    [MessagePackObject]
    public class MessageRequestRegister : AMessageRequest
    {
        [Key(1)]
        public string UserName { get; set; }
        [Key(2)]
        public string Password { get; set; }
    }
}
