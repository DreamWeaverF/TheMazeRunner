using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazeRunner
{
    public static class ClientLaunch
    {
        private static string NetUrl = "http://127.0.0.1:20001/";

        
        public static void Start(LAUNCH_ENVIRONMENT _environment)
        {
            LogHelper.Log($"Start Client ##Environment: {_environment}");

            //for (ushort i = 10000; i < 20000; i += 128)
            {
                ushort i = 100;
                Debug.Log($"Sender I: {i}");
                //MessageHelper.SendHttpMessage<MessageRequestLogin, MessageResponseLogin>(new MessageRequestLogin()
                //{
                //    UserName = "111",
                //    Password = "222",
                //}, NetUrl);
            }


        }
    }
}


