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
        private DatabaseConfigSO databaseConfig;
        [SerializeField]
        private List<ADatabaseTableSO> databaseTableSOs = new List<ADatabaseTableSO>();
        //最大同时连接数
        private const int maxConnectCount = 100;
        private Queue<MySqlConnection> mySqlConnectionList = new Queue<MySqlConnection>();
        //
        private Queue<ETTask<MySqlConnection>> requestCallbacks = new Queue<ETTask<MySqlConnection>>();
        void Start()
        {
            string _connectionInfo = $"host={databaseConfig.Host};port={databaseConfig.Port};user id={databaseConfig.UserId};password={databaseConfig.Password};database={databaseConfig.Database}; character set={databaseConfig.CharacterSet}; ConnectionLifeTime={databaseConfig.ConnectionLifeTime}; SslMode={databaseConfig.SslMode};";
            
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
                databaseTableSOs[_i].Init(this);
            }
        }

        void OnDestroy()
        {
            
        }

        void Update()
        {
            if(mySqlConnectionList.Count <= 0)
            {
                return;
            }    
            if(requestCallbacks.Count <= 0)
            {
                return;
            }
            ETTask<MySqlConnection> _task = requestCallbacks.Dequeue();
            MySqlConnection _connection = mySqlConnectionList.Dequeue();
            _connection.Open();
            _task.SetResult(_connection);
        }

        private ETTask<MySqlConnection> FetchConnection()
        {
            ETTask<MySqlConnection> _task = ETTask<MySqlConnection>.Create();
            requestCallbacks.Enqueue(_task);
            return _task;
        }
        //
        private void RecycleConnection(MySqlConnection _connection)
        {
            _connection.Close();
            mySqlConnectionList.Enqueue(_connection);
        }

        private MySqlDbType CoverTypeToDbType<T1>(T1 _typeValue)
        {
            MySqlDbType _dbType;
            switch (_typeValue.GetType().Name)
            {
                case "String":
                    _dbType = MySqlDbType.VarChar;
                    break;
                case "Int32":
                    _dbType = MySqlDbType.Int32;
                    break;
                case "Int64":
                    _dbType = MySqlDbType.Int64;
                    break;
                case "Byte[]":
                    _dbType = MySqlDbType.Blob;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return _dbType;
        }

        public async Task<long> SelectCount<T1>(string _tableName, string _keyName1, T1 _keyValue1)
        {
            MySqlConnection _connection = await FetchConnection();
            MySqlCommand _cmd = _connection.CreateCommand();
            _cmd.CommandTimeout = 60;
            _cmd.CommandText = $"SELECT COUNT({_keyName1}) AS {_keyName1} FROM {_tableName} WHERE {_keyName1}= @KeyValue;";
            _cmd.Parameters.Add("@KeyValue", CoverTypeToDbType(_keyValue1)).Value = _keyValue1;
            long _count = 0;
            using (DbDataReader _dbReader = await _cmd.ExecuteReaderAsync())
            {
                if (await _dbReader.ReadAsync())
                {
                    _count = await _dbReader.GetFieldValueAsync<long>(0);
                }
            }
            RecycleConnection(_connection);
            return _count;
        }

        public async Task<long> SelectTotalCount(string _tableName)
        {
            MySqlConnection _connection = await FetchConnection();
            MySqlCommand _cmd = _connection.CreateCommand();
            _cmd.CommandTimeout = 60;
            _cmd.CommandText = $"SELECT COUNT(1) FROM {_tableName};";
            long _count = 0;
            using (DbDataReader _dbReader = await _cmd.ExecuteReaderAsync())
            {
                if (await _dbReader.ReadAsync())
                {
                    _count = await _dbReader.GetFieldValueAsync<long>(0);
                }
            }
            RecycleConnection(_connection);
            return _count;
        }

        public async void TruncateTable(string _tableName)
        {
            MySqlConnection _connection = await FetchConnection();
            MySqlCommand _cmd = _connection.CreateCommand();
            _cmd.CommandTimeout = 60;
            _cmd.CommandText = $"TRUNCATE Table {_tableName}";
            await _cmd.ExecuteNonQueryAsync();
            RecycleConnection(_connection);
        }

        public async Task<bool> InsertIntoCmd<T1,T2>(string _tableName, string _keyName1, string _keyName2, T1 _keyValue1, T2 _keyValue2)
        {
            MySqlConnection _connection = await FetchConnection();
            MySqlCommand _cmd = _connection.CreateCommand();
            _cmd.CommandTimeout = 60;
            _cmd.CommandText = $"INSERT INTO {_tableName} ({_keyName1},{_keyName2}) VALUES (@{_keyName1}, @{_keyName2});";
            _cmd.Parameters.Add($"@{_keyName1}", CoverTypeToDbType(_keyValue1)).Value = _keyValue1;
            _cmd.Parameters.Add($"@{_keyName2}", CoverTypeToDbType(_keyValue2)).Value = _keyValue2;
            await _cmd.ExecuteNonQueryAsync();
            RecycleConnection(_connection);
            return true;
        }

        public async Task<bool> InsertIntoCmd<T1, T2, T3>(string _tableName, string _keyName1, string _keyName2, string _keyName3, T1 _keyValue1, T2 _keyValue2, T3 _keyValue3)
        {
            MySqlConnection _connection = await FetchConnection();
            MySqlCommand _cmd = _connection.CreateCommand();
            _cmd.CommandTimeout = 60;
            _cmd.CommandText = $"INSERT INTO {_tableName} ({_keyName1},{_keyName2},{_keyName3}) VALUES (@{_keyName1}, @{_keyName2},@{_keyName3});";
            _cmd.Parameters.Add($"@{_keyName1}", CoverTypeToDbType(_keyValue1)).Value = _keyValue1;
            _cmd.Parameters.Add($"@{_keyName2}", CoverTypeToDbType(_keyValue2)).Value = _keyValue2;
            _cmd.Parameters.Add($"@{_keyName3}", CoverTypeToDbType(_keyValue3)).Value = _keyValue3;
            await _cmd.ExecuteNonQueryAsync();
            RecycleConnection(_connection);
            return true;
        }

        public async Task<bool> UpdateCmd<T1,T2>(string _tableName, string _keyName1, string _keyName2,T1 _keyValue1, T2 _keyValue2)
        {
            MySqlConnection _connection = await FetchConnection();
            MySqlCommand _cmd = _connection.CreateCommand();
            _cmd.CommandTimeout = 60;
            _cmd.CommandText = $"UPDATE {_tableName} set {_keyName1} = @{_keyName1}, {_keyName2} = @{_keyName2} WHERE {_keyName1} = @{_keyName1};";
            _cmd.Parameters.Add($"@{_keyName1}", CoverTypeToDbType(_keyValue1)).Value = _keyValue1;
            _cmd.Parameters.Add($"@{_keyName2}", CoverTypeToDbType(_keyValue2)).Value = _keyValue2;
            await _cmd.ExecuteNonQueryAsync();
            RecycleConnection(_connection);
            return true;
        }

        public async Task<bool> UpdateCmd<T1, T2, T3>(string _tableName, string _keyName1, string _keyName2, string _keyName3, T1 _keyValue1, T2 _keyValue2, T3 _keyValue3)
        {
            MySqlConnection _connection = await FetchConnection();
            MySqlCommand _cmd = _connection.CreateCommand();
            _cmd.CommandTimeout = 60;
            _cmd.CommandText = $"UPDATE {_tableName} set {_keyName1} = @{_keyName1}, {_keyName2} = @{_keyName2}, {_keyName3} = @{_keyName3} WHERE {_keyName1} = @{_keyName1};";
            _cmd.Parameters.Add($"@{_keyName1}", CoverTypeToDbType(_keyValue1)).Value = _keyValue1;
            _cmd.Parameters.Add($"@{_keyName2}", CoverTypeToDbType(_keyValue2)).Value = _keyValue2;
            _cmd.Parameters.Add($"@{_keyName3}", CoverTypeToDbType(_keyValue3)).Value = _keyValue3;
            await _cmd.ExecuteNonQueryAsync();
            RecycleConnection(_connection);
            return true;
        }

        public async Task<T2> SelectCmd<T1,T2>(string _tableName,string _keyName1,string _keyName2,T1 _keyValue1)
        {
            MySqlConnection _connection = await FetchConnection();
            MySqlCommand _cmd = _connection.CreateCommand();
            _cmd.CommandTimeout = 60;
            _cmd.CommandText = $"SELECT {_keyName1},{_keyName2} from {_tableName} where {_keyName1}= @KeyValue;";
            _cmd.Parameters.Add("@KeyValue", CoverTypeToDbType(_keyValue1)).Value = _keyValue1;

            T2 _keyValue2 = default;
            using (DbDataReader _dbReader = await _cmd.ExecuteReaderAsync())
            {
                if (await _dbReader.ReadAsync())
                {
                    _keyValue2 = await _dbReader.GetFieldValueAsync<T2>(1);
                }
            }
            RecycleConnection(_connection);
            return _keyValue2;
        }

        public async Task<(T2,T3)> SelectCmd<T1, T2, T3>(string _tableName, string _keyName1, string _keyName2, string _keyName3, T1 _keyValue1)
        {
            MySqlConnection _connection = await FetchConnection();
            MySqlCommand _cmd = _connection.CreateCommand();
            _cmd.CommandTimeout = 60;
            _cmd.CommandText = $"SELECT {_keyName1},{_keyName2},{_keyName3} from {_tableName} where {_keyName1}= @KeyValue;";
            _cmd.Parameters.Add("@KeyValue", CoverTypeToDbType(_keyValue1)).Value = _keyValue1;

            T2 _keyValue2 = default;
            T3 _keyValue3 = default;

            using (DbDataReader _dbReader = await _cmd.ExecuteReaderAsync())
            {
                if (await _dbReader.ReadAsync())
                {
                    _keyValue2 = await _dbReader.GetFieldValueAsync<T2>(1);
                    _keyValue3 = await _dbReader.GetFieldValueAsync<T3>(2);
                }
            }
            RecycleConnection(_connection);
            return (_keyValue2, _keyValue3);
        }
    }
}
