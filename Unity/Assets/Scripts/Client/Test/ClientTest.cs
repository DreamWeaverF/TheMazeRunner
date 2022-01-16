using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazeRunner
{
    public class ClientTest : MonoBehaviour
    {
        [SerializeField]
        private MessageResponseLoginHandler responLoginHandler;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                onClick();
            }
        }

        async void onClick()
        {
            MessageResponseLogin _response = await responLoginHandler.OnSendMessage(new MessageRequestLogin()
            {
                UserName = "xxx",
                Password = "111",
            });
        }
    }
}
