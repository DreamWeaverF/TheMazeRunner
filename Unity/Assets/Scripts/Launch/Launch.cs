using System;
using System.IO;
using System.Reflection;
using UnityEngine;

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

        void Start()
        {
            switch (Model)
            {
                case LAUNCH_MODEL.CLIENT:
                    StartAssemblyFile("Client");
                    break;
                case LAUNCH_MODEL.LOGIN_SERVER:
                    StartAssemblyFile("LoginServer");
                    break;
                case LAUNCH_MODEL.GATE_SERVER:
                    StartAssemblyFile("GateServer");
                    break;
                case LAUNCH_MODEL.CENTER_SERVER:
                    StartAssemblyFile("CenterServer");
                    break;
                case LAUNCH_MODEL.MAP_SERVER:
                    StartAssemblyFile("MapServer");
                    break;
                case LAUNCH_MODEL.USER_SERVER:
                    StartAssemblyFile("UserServer");
                    break;
                case LAUNCH_MODEL.MAIL_SERVER:
                    StartAssemblyFile("MailServer");
                    break;
                case LAUNCH_MODEL.CHAT_SERVER:
                    StartAssemblyFile("ChatServer");
                    break;
                case LAUNCH_MODEL.TOTAL:
                    StartAssemblyFile("CenterServer");
                    StartAssemblyFile("UserServer");
                    StartAssemblyFile("MailServer");
                    StartAssemblyFile("ChatServer");
                    StartAssemblyFile("MapServer");
                    StartAssemblyFile("GateServer");
                    StartAssemblyFile("LoginServer");
                    StartAssemblyFile("Client");
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void StartAssemblyFile(string _fileName)
        {
            byte[] _dllByte = default;
            byte[] _pdbByte = default;
            switch (Environment)
            {
                case LAUNCH_ENVIRONMENT.DEV:
                    _dllByte = File.ReadAllBytes(string.Format(PathHelper.DevDllFilePath, _fileName));
                    _pdbByte = File.ReadAllBytes(string.Format(PathHelper.DevPDBFilePath, _fileName));
                    break;
                case LAUNCH_ENVIRONMENT.QA:
                case LAUNCH_ENVIRONMENT.RELEASE:
                    _dllByte = File.ReadAllBytes(string.Format(PathHelper.ReleaseDllFilePath, _fileName));
                    _pdbByte = File.ReadAllBytes(string.Format(PathHelper.ReleasePDBFilePath, _fileName));
                    break;
            }
            if(_dllByte == default)
            {
                LogHelper.LogError($"获取Dll失败:{_fileName}");
                return;
            }
            if (_pdbByte == default)
            {
                LogHelper.LogError($"获取PDB失败:{_fileName}");
                return;
            }
            Assembly _assembly = Assembly.Load(_dllByte, _pdbByte);
            Type _launch = _assembly.GetType($"TheMazeRunner.{_fileName}Launch");
            MethodBase _method = _launch.GetMethod("Start");
            _method.Invoke(_launch, new object[] { Environment });
        }
    }
}

