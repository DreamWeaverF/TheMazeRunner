using System;
using UnityEngine;

namespace TheMazeRunner
{
    public enum APP_ENVIRONMENT
    {
        None,
        Dev,
        Qa,
        Release,
    }

    public enum APP_MODEL
    {
        None,
        Total,
        Client,
        GateServer,
        CenterServer,
        LoginServer,
        UserServer,
        MapServer,
        MailServer,
        ChatServer,
    }


    public class Launch : MonoBehaviour
    {
        //[SerializeField]
        //private LaunchSettingSO settingSO;

        void Start()
        {
            //string[] CommandLineArgs = Environment.GetCommandLineArgs();

            //for (int i = 0; i < CommandLineArgs.Length; i++)
            //{
            //    Debug.Log(CommandLineArgs[i]);
            //}
        }

        //IEnumerator Start()
        //{
        //    int _serviceType = (int)(EnumServiceType.Client | EnumServiceType.CenterServer);
        //    int _targetType = (int)(_serviceType ^ (int)EnumServiceType.Client);

        //    //launchConfigSO.Environment = environment;
        //    //launchConfigSO.Model = model;
        //    //launchConfigSO.Index = index;

        //    //AsyncOperation _loadOpear = SceneManager.LoadSceneAsync("LoginServer",LoadSceneMode.Additive);
        //    //yield return _loadOpear;

        //    yield return 1000;

        //    //_loadOpear = SceneManager.LoadSceneAsync("Client", LoadSceneMode.Additive);
        //    //yield return _loadOpear;
        //    //switch (Model)
        //    //{
        //    //    case LAUNCH_MODEL.CLIENT:
        //    //        LoadScene("Client");
        //    //        break;
        //    //    case LAUNCH_MODEL.LOGIN_SERVER:
        //    //        LoadScene("LoginServer");
        //    //        break;
        //    //    case LAUNCH_MODEL.GATE_SERVER:
        //    //        LoadScene("GateServer");
        //    //        break;
        //    //    case LAUNCH_MODEL.CENTER_SERVER:
        //    //        LoadScene("CenterServer");
        //    //        break;
        //    //    case LAUNCH_MODEL.MAP_SERVER:
        //    //        LoadScene("MapServer");
        //    //        break;
        //    //    case LAUNCH_MODEL.USER_SERVER:
        //    //        LoadScene("UserServer");
        //    //        break;
        //    //    case LAUNCH_MODEL.MAIL_SERVER:
        //    //        LoadScene("MailServer");
        //    //        break;
        //    //    case LAUNCH_MODEL.CHAT_SERVER:
        //    //        LoadScene("ChatServer");
        //    //        break;
        //    //    case LAUNCH_MODEL.TOTAL:
        //    //        //LoadScene("CenterServer");
        //    //        //LoadScene("UserServer");
        //    //        //LoadScene("MailServer");
        //    //        //LoadScene("ChatServer");
        //    //        //LoadScene("MapServer");
        //    //        //LoadScene("GateServer");
        //    //        LoadScene(new List<string> { 
        //    //            "LoginServer",
        //    //            "Client" 
        //    //        });
        //    //        break;
        //    //}
        //}

        //IEnumerator LoadScene(string _fileName)
        //{
        //    AsyncOperation _loadOpear = SceneManager.LoadSceneAsync(_fileName);
        //    yield return _loadOpear;
        //}

        //IEnumerator LoadScene(List<string> _fileNameList)
        //{
        //    for(int _index = 0; _index < _fileNameList.Count; _index++)
        //    {
        //        yield return LoadScene(_fileNameList[_index]);
        //    }
        //}
    }
}

