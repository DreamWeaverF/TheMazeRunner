using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using UnityEngine;

namespace TheMazeRunner
{
    public class DatabaseController : MonoBehaviour
    {
        [SerializeField]
        private List<ADatabaseTableSO> databaseTableSOs = new List<ADatabaseTableSO>();

        //最大同时连接数
        private const int maxConnectCount = 100;
        //线程锁
        private object lockObject = new object();

        private Queue<MySqlConnection> mySqlConnectionList = default;
        // Start is called before the first frame update
        void Start()
        {
            mySqlConnectionList = new Queue<MySqlConnection>();

            string _connectionInfo = $"host=47.108.253.245;port={3306};user id=root;password=120120@6m;database=db; character set=utf8; ConnectionLifeTime=10; SslMode=none;";
            
            for (int _i = 0; _i < maxConnectCount; _i++)
            {
                try
                {
                    MySqlConnection _connection = new MySqlConnection(_connectionInfo);
                    mySqlConnectionList.Enqueue(_connection);
                }
                catch (Exception _e)
                {
                    //
                    Debug.LogError(_e);
                }
            }

            for(int _i = 0; _i < databaseTableSOs.Count; _i++)
            {
                databaseTableSOs[_i].FetchConnectionEvent += OnFetchConnection;
                databaseTableSOs[_i].RecycleConnectionEvent += OnRecycleConnection;
            }
        }

        private void OnDestroy()
        {
            for (int _i = 0; _i < databaseTableSOs.Count; _i++)
            {
                databaseTableSOs[_i].FetchConnectionEvent -= OnFetchConnection;
                databaseTableSOs[_i].RecycleConnectionEvent -= OnRecycleConnection;
            }
        }

        public async Task<MySqlConnection> OnFetchConnection()
        {
            while (!CheckConnect())
            {
                await Task.Delay(1000);
            }
            lock (lockObject)
            {
                MySqlConnection _connection = mySqlConnectionList.Dequeue();
                _connection.Open();
                return _connection;
            }
        }
        //
        private void OnRecycleConnection(MySqlConnection _connection)
        {
            lock (lockObject)
            {
                _connection.Close();
                mySqlConnectionList.Enqueue(_connection);
            }
        }

        private bool CheckConnect()
        {
            lock (lockObject)
            {
                return mySqlConnectionList.Count > 0;
            }
        }
    }
}
