using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TheMazeRunner
{
    public class ClientTest : MonoBehaviour
    {

        [SerializeField]
        private AMessageRequestSenderSO requestSenderSO;

        void Start()
        {
            UITest uiText = new UITest();

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                onClick();
            }
        }

        void onClick()
        {
            //long _count = await databaseTableTest.SelectCountFromTableName();
            //MessageResponseLogin _response = await requestSenderSO.SendMessage<MessageRequestLogin,MessageResponseLogin>(new MessageRequestLogin()
            //{
            //    UserName = "xxx",
            //    Password = "111",
            //});
            //Log.Debug($"Sender{_response.IpAddress}");
        }
    }
}
