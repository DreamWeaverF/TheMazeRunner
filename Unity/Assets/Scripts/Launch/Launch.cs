using ET;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheMazeRunner
{
    public enum LAUNCH_ENVIRONMENT
    {
        NONE,
        DEV,
        QA,
        RELEASE,
    }

    public enum LAUNCH_MODEL
    {
        NONE,
        TOTAL,
        CLIENT,
        LOGIN_SERVER,
        GATE_SERVER,
        CENTER_SERVER,
        MAP_SERVER,
        USER_SERVER,
        MAIL_SERVER,
        CHAT_SERVER,
    }

    public class Launch : MonoBehaviour
    {
        public LAUNCH_ENVIRONMENT Environment;
        public LAUNCH_MODEL Model;

        IEnumerator Start()
        {
            AsyncOperation _loadOpear = SceneManager.LoadSceneAsync("LoginServer",LoadSceneMode.Additive);
            yield return _loadOpear;

            yield return 1000;

            _loadOpear = SceneManager.LoadSceneAsync("Client", LoadSceneMode.Additive);
            yield return _loadOpear;
            //switch (Model)
            //{
            //    case LAUNCH_MODEL.CLIENT:
            //        LoadScene("Client");
            //        break;
            //    case LAUNCH_MODEL.LOGIN_SERVER:
            //        LoadScene("LoginServer");
            //        break;
            //    case LAUNCH_MODEL.GATE_SERVER:
            //        LoadScene("GateServer");
            //        break;
            //    case LAUNCH_MODEL.CENTER_SERVER:
            //        LoadScene("CenterServer");
            //        break;
            //    case LAUNCH_MODEL.MAP_SERVER:
            //        LoadScene("MapServer");
            //        break;
            //    case LAUNCH_MODEL.USER_SERVER:
            //        LoadScene("UserServer");
            //        break;
            //    case LAUNCH_MODEL.MAIL_SERVER:
            //        LoadScene("MailServer");
            //        break;
            //    case LAUNCH_MODEL.CHAT_SERVER:
            //        LoadScene("ChatServer");
            //        break;
            //    case LAUNCH_MODEL.TOTAL:
            //        //LoadScene("CenterServer");
            //        //LoadScene("UserServer");
            //        //LoadScene("MailServer");
            //        //LoadScene("ChatServer");
            //        //LoadScene("MapServer");
            //        //LoadScene("GateServer");
            //        LoadScene(new List<string> { 
            //            "LoginServer",
            //            "Client" 
            //        });
            //        break;
            //}
        }

        IEnumerator LoadScene(string _fileName)
        {
            AsyncOperation _loadOpear = SceneManager.LoadSceneAsync(_fileName);
            yield return _loadOpear;
        }

        IEnumerator LoadScene(List<string> _fileNameList)
        {
            for(int _index = 0; _index < _fileNameList.Count; _index++)
            {
                yield return LoadScene(_fileNameList[_index]);
            }
        }

        void Update()
        {
            ThreadSynchronizationContext.Instance.Update();
        }
    }
}

