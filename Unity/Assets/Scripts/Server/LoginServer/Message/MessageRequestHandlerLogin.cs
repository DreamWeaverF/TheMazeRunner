using System.Threading.Tasks;
using UnityEngine;

namespace TheMazeRunner
{
    [CreateAssetMenu(fileName = "MessageRequestHandlerLogin", menuName = "ScriptableObjects/Message/MessageRequestHandlerLogin")]
    public class MessageRequestHandlerLogin : AMessageRequestHandler<MessageRequestLogin, MessageResponseLogin>
    {
        protected override async Task OnHandMessage(MessageRequestLogin _request, MessageResponseLogin _response)
        {
            await Task.CompletedTask;
            _response.RpcId = _request.RpcId;
            _response.IpAddress = "xxx";
            _response.UserId = 1111;
        }
    }
}
