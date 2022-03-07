using MessagePack;

namespace TheMazeRunner
{
    public class MessageRequestLogin : AMessageRequest
    {
        [Key(1)]
        public string UserName { get; set; }
        [Key(2)]
        public string Password { get; set; }
    }

    public class MessageResponseLogin : AMessageResponse
    {
        [Key(1)]
        public long UserId { get; set; }
        [Key(2)]
        public long SessonKey { get; set; }
        [Key(3)]
        public string IpAddress { get; set; }
    }
}
