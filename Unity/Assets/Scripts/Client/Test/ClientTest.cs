using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazeRunner
{
    public class ClientTest : MonoBehaviour
    {
        [SerializeField]
        private MessageRequestSenderLogin responLoginHandler;
        [SerializeField]
        private DatabaseTableRank databaseTableTest;
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                onClick();
            }
        }

        async void onClick()
        {
            long _count = await databaseTableTest.SelectCountFromTableName();
            MessageResponseLogin _response = await responLoginHandler.OnSendMessage(new MessageRequestLogin()
            {
                UserName = "xxx",
                Password = "111",
            });
        }
    }
}
