using MessagePack;

namespace TheMazeRunner
{
    [MessagePackObject]
    public class MessageResponseRegister : AMessageResponse
    {
        [Key(1)]
        public long UserId { get; set; }
        [Key(2)]
        public long SessonKey { get; set; }
        [Key(3)]
        public string IpAddress { get; set; }
    }
}
