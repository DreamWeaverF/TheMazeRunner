using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazeRunner
{
    [AutoGenSO(GenPath = "DataBase/Config")]
    public class DatabaseConfigSO : ScriptableObject
    {
        public string Host = "127.0.0.1";
        public int Port = 3306;
        public string UserId = "xxx";
        public string Password = "xxx";
        public string Database = "db";
        public string CharacterSet = "utf8";
        public int ConnectionLifeTime = 10;
        public string SslMode = "none";
    }
}
