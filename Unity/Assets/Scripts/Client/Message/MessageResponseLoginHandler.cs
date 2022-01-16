using UnityEngine;

namespace TheMazeRunner
{
    [CreateAssetMenu(fileName = "MessageResponseLoginSO", menuName = "ScriptableObjects/Message/MessageResponseLoginHandler")]
    public class MessageResponseLoginHandler : AMessageResponseHandler<MessageRequestLogin, MessageResponseLogin>
    {
        protected override void OnHandMessage(MessageResponseLogin _response)
        {

        }
    }
}
