using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace TheMazeRunner
{
    [AutoGenSO(GenPath = "LoginServer/Handler")]
    public class MessageRequestHandlerLoginSO : AMessageRequestHandlerSO<MessageRequestLogin, MessageResponseLogin>
    {
        public override async Task OnHandMessage(MessageRequestLogin _request)
        {
            await Task.CompletedTask;
            _response.RpcId = _request.RpcId;
            _response.IpAddress = "xxx";
            _response.UserId = 1111;
        }
    }
}
