using MessagePack;

namespace TheMazeRunner
{

    [Union(100, typeof(MessageRequestLogin))]
    [Union(101, typeof(MessageResponseLogin))]

    [Union(102, typeof(MessageResponseRegister))]
    [Union(103, typeof(MessageRequestRegister))]
    public interface IMessage 
    {

    }

    public delegate void DelegateSendMessageEvent(IMessage _message);
}
