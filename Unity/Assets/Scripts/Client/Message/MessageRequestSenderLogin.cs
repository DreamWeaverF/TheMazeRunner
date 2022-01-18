using UnityEngine;

namespace TheMazeRunner
{
    [CreateAssetMenu(fileName = "MessageRequestSenderLogin", menuName = "ScriptableObjects/Message/MessageRequestSenderLogin")]
    public class MessageRequestSenderLogin : AMessageRequestSender<MessageRequestLogin, MessageResponseLogin>
    {
        protected override void OnHandMessage(MessageResponseLogin _response)
        {
            Debug.Log($"Result {_response.UserId}");
        }
    }
}
