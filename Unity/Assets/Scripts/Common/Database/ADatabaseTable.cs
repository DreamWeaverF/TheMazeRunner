using MessagePack;
using MySql.Data.MySqlClient;
using System;
using System.Data.Common;
using System.Threading.Tasks;
using UnityEngine;

namespace TheMazeRunner
{
    public delegate Task<MySqlConnection> DelegateFetchConnectionEvent();
    public delegate void DelegateRecycleConnectionEvent(MySqlConnection _connection);

    public abstract class ADatabaseTable<T1, T2, T3> : ADatabaseTableSO
    {
        protected abstract string keyName2 { get; }
        protected abstract string keyName3 { get; }
        //
        public async Task<bool> InsertIntoCmd(T1 _keyValue1, T2 _keyValue2,T3 _keyValue3)
        {
            IAsyncResult _result = FetchConnectionEvent.BeginInvoke(null, null);
            while (_result.IsCompleted == false)
            {
                await Task.Delay(1000);
            }
            MySqlConnection _connection = FetchConnectionEvent.EndInvoke(_result).Result;
            MySqlCommand _cmd = _connection.CreateCommand();
            _cmd.CommandTimeout = 60;
            _cmd.CommandText = $"INSERT INTO {tableName} ({keyName1},{keyName2},{keyName3}) VALUES (@{keyName1}, @{keyName2},@{keyName3});";
            _cmd.Parameters.Add($"@{keyName1}", CoverTypeToDbType(_keyValue1)).Value = _keyValue1;
            _cmd.Parameters.Add($"@{keyName2}", CoverTypeToDbType(_keyValue2)).Value = _keyValue2;
            _cmd.Parameters.Add($"@{keyName3}", CoverTypeToDbType(_keyValue3)).Value = _keyValue3;
            await _cmd.ExecuteNonQueryAsync();
            RecycleConnectionEvent.Invoke(_connection);
            return true;
        }

        public async Task<T2> SelectCmd(T1 _keyValue1)
        {
            IAsyncResult _result = FetchConnectionEvent.BeginInvoke(null, null);
            while (_result.IsCompleted == false)
            {
                await Task.Delay(1000);
            }
            MySqlConnection _connection = FetchConnectionEvent.EndInvoke(_result).Result;
            MySqlCommand _cmd = _connection.CreateCommand();
            _cmd.CommandTimeout = 60;
            _cmd.CommandText = $"SELECT {keyName1},{keyName2},{keyName3} from {tableName} where {keyName1}= @KeyValue;";
            _cmd.Parameters.Add("@KeyValue", CoverTypeToDbType(_keyValue1)).Value = _keyValue1;

            using (DbDataReader _dbReader = await _cmd.ExecuteReaderAsync())
            {
                if (await _dbReader.ReadAsync())
                {
                    T2 _outData = await GetByteFileValueAsync<T2>(_dbReader, 1);
                    RecycleConnectionEvent.Invoke(_connection);
                    return _outData;
                }
            }
            return default(T2);
        }

        public async Task<bool> UpdateCmd(T1 _keyValue1, T2 _keyValue2,T3 _keyValue3)
        {
            IAsyncResult _result = FetchConnectionEvent.BeginInvoke(null, null);
            while (_result.IsCompleted == false)
            {
                await Task.Delay(1000);
            }
            MySqlConnection _connection = FetchConnectionEvent.EndInvoke(_result).Result;
            MySqlCommand _cmd = _connection.CreateCommand();
            _cmd.CommandTimeout = 60;
            _cmd.CommandText = $"UPDATE {tableName} set {keyName1} = @{keyName1}, {keyName2} = @{keyName2}, {keyName3} = @{keyName3} WHERE {keyName1} = @{keyName1};";
            _cmd.Parameters.Add($"@{keyName1}", CoverTypeToDbType(_keyValue1)).Value = _keyValue1;
            _cmd.Parameters.Add($"@{keyName2}", CoverTypeToDbType(_keyValue2)).Value = _keyValue2;
            _cmd.Parameters.Add($"@{keyName3}", CoverTypeToDbType(_keyValue3)).Value = _keyValue3;
            await _cmd.ExecuteNonQueryAsync();
            RecycleConnectionEvent.Invoke(_connection);
            return true;
        }
    }


    public abstract class ADatabaseTable<T1,T2> : ADatabaseTableSO
    {
        protected abstract string keyName2 { get; }
        //
        public async Task<bool> InsertIntoCmd(T1 _keyValue1,T2 _keyValue2)
        {
            IAsyncResult _result = FetchConnectionEvent.BeginInvoke(null, null);
            while (_result.IsCompleted == false)
            {
                await Task.Delay(1000);
            }
            MySqlConnection _connection = FetchConnectionEvent.EndInvoke(_result).Result;
            MySqlCommand _cmd = _connection.CreateCommand();
            _cmd.CommandTimeout = 60;
            _cmd.CommandText = $"INSERT INTO {tableName} ({keyName1},{keyName2}) VALUES (@{keyName1}, @{keyName2});";
            _cmd.Parameters.Add($"@{keyName1}", CoverTypeToDbType(_keyValue1)).Value = _keyValue1;
            _cmd.Parameters.Add($"@{keyName2}", CoverTypeToDbType(_keyValue2)).Value = _keyValue2;
            await _cmd.ExecuteNonQueryAsync();
            RecycleConnectionEvent.Invoke(_connection);
            return true;
        }

        public async Task<T2> SelectCmd(T1 _keyValue1)
        {
            IAsyncResult _result = FetchConnectionEvent.BeginInvoke(null, null);
            while (_result.IsCompleted == false)
            {
                await Task.Delay(1000);
            }
            MySqlConnection _connection = FetchConnectionEvent.EndInvoke(_result).Result;
            MySqlCommand _cmd = _connection.CreateCommand();
            _cmd.CommandTimeout = 60;
            _cmd.CommandText = $"SELECT {keyName1},{keyName2} from {tableName} where {keyName1}= @KeyValue;";
            _cmd.Parameters.Add("@KeyValue", CoverTypeToDbType(_keyValue1)).Value = _keyValue1;

            using (DbDataReader _dbReader = await _cmd.ExecuteReaderAsync())
            {
                if (await _dbReader.ReadAsync())
                {
                    T2 _outData = await GetByteFileValueAsync<T2>(_dbReader,1);
                    RecycleConnectionEvent.Invoke(_connection);
                    return _outData;
                }
            }
            return default(T2);
        }

        public async Task<bool> UpdateCmd(T1 _keyValue1, T2 _keyValue2)
        {
            IAsyncResult _result = FetchConnectionEvent.BeginInvoke(null, null);
            while (_result.IsCompleted == false)
            {
                await Task.Delay(1000);
            }
            MySqlConnection _connection = FetchConnectionEvent.EndInvoke(_result).Result;
            MySqlCommand _cmd = _connection.CreateCommand();
            _cmd.CommandTimeout = 60;
            _cmd.CommandText = $"UPDATE {tableName} set {keyName1} = @{keyName1}, {keyName2} = @{keyName2} WHERE {keyName1} = @{keyName1};";
            _cmd.Parameters.Add($"@{keyName1}", CoverTypeToDbType(_keyValue1)).Value = _keyValue1;
            _cmd.Parameters.Add($"@{keyName2}", CoverTypeToDbType(_keyValue2)).Value = _keyValue2;
            await _cmd.ExecuteNonQueryAsync();
            RecycleConnectionEvent.Invoke(_connection);
            return true;
        }
    }

    public abstract class ADatabaseTableSO : ScriptableObject
    {
        public DelegateFetchConnectionEvent FetchConnectionEvent;
        public DelegateRecycleConnectionEvent RecycleConnectionEvent;
        protected abstract string tableName { get;}
        protected abstract string keyName1 { get; }

        public async Task<long> SelectCount<T1>(T1 _keyValue)
        {
            IAsyncResult _result = FetchConnectionEvent.BeginInvoke(null, null);
            while (_result.IsCompleted == false)
            {
                await Task.Delay(1000);
            }
            MySqlConnection _connection = FetchConnectionEvent.EndInvoke(_result).Result;
            MySqlCommand _cmd = _connection.CreateCommand();
            _cmd.CommandTimeout = 60;
            _cmd.CommandText = $"SELECT COUNT({keyName1}) AS {keyName1} FROM {tableName} WHERE {keyName1}= @KeyValue;";
            _cmd.Parameters.Add("@KeyValue", CoverTypeToDbType(_keyValue)).Value = _keyValue;

            using (DbDataReader _dbReader = await _cmd.ExecuteReaderAsync())
            {
                if (await _dbReader.ReadAsync())
                {
                    long _count = await _dbReader.GetFieldValueAsync<long>(0);
                    RecycleConnectionEvent.Invoke(_connection);
                    return _count;
                }
            }
            return 0;
        }

        public async Task<long> SelectCountFromTableName()
        {
            IAsyncResult _result = FetchConnectionEvent.BeginInvoke(null,null);
            while (_result.IsCompleted == false)
            {
                await Task.Delay(1000);
            }
            MySqlConnection _connection = FetchConnectionEvent.EndInvoke(_result).Result;
            MySqlCommand _cmd = _connection.CreateCommand();
            _cmd.CommandTimeout = 60;
            _cmd.CommandText = $"SELECT COUNT(1) FROM {tableName};";

            using (DbDataReader _dbReader = await _cmd.ExecuteReaderAsync())
            {
                if (await _dbReader.ReadAsync())
                {
                    long _count = await _dbReader.GetFieldValueAsync<long>(0);
                    RecycleConnectionEvent.Invoke(_connection);
                    return _count;
                }
            }
            return 0;
        }

        public async void TruncateTable()
        {
            IAsyncResult _result = FetchConnectionEvent.BeginInvoke(null, null);
            while (_result.IsCompleted == false)
            {
                await Task.Delay(1000);
            }
            MySqlConnection _connection = FetchConnectionEvent.EndInvoke(_result).Result;
            MySqlCommand _cmd = _connection.CreateCommand();
            _cmd.CommandTimeout = 60;
            _cmd.CommandText = $"TRUNCATE Table {tableName}";
            await _cmd.ExecuteNonQueryAsync();
            RecycleConnectionEvent(_connection);
        }

        protected async Task<T1> GetByteFileValueAsync<T1>(DbDataReader _dbDataReader, int _index)
        {
            byte[] _byteData = await _dbDataReader.GetFieldValueAsync<byte[]>(_index);
            return MessagePackSerializer.Deserialize<T1>(_byteData);
        }

        protected MySqlDbType CoverTypeToDbType<T1>(T1 _typeValue)
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
    }
}
