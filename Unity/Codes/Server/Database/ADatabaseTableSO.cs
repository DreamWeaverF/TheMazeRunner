using System.Threading.Tasks;
using UnityEngine;

namespace TheMazeRunner
{
    public abstract class ADatabaseTableSO<T1, T2, T3> : ADatabaseTableSO 
    {
        protected abstract string keyName2 { get; }
        protected abstract string keyName3 { get; }
        //
        protected async Task<bool> InsertIntoCmd(T1 _keyValue1, T2 _keyValue2,T3 _keyValue3)
        {
            return await databaseController.InsertIntoCmd(tableName, keyName1, keyName2, keyName3, _keyValue1, _keyValue2, _keyValue3);
        }

        protected async Task<bool> UpdateCmd(T1 _keyValue1, T2 _keyValue2, T3 _keyValue3)
        {
            return await databaseController.UpdateCmd(tableName, keyName1, keyName2, keyName3, _keyValue1, _keyValue2, _keyValue3);
        }

        protected async Task<(T2,T3)> SelectCmd(T1 _keyValue1)
        {
            return await databaseController.SelectCmd<T1,T2,T3>(tableName, keyName1, keyName2, keyName3, _keyValue1);
        }
    }


    public abstract class ADatabaseTable<T1,T2> : ADatabaseTableSO
    {
        protected abstract string keyName2 { get; }
        //
        protected async Task<bool> InsertIntoCmd(T1 _keyValue1,T2 _keyValue2)
        {
            return await databaseController.InsertIntoCmd(tableName,keyName1,keyName2,_keyValue1,_keyValue2);
        }

        protected async Task<bool> UpdateCmd(T1 _keyValue1, T2 _keyValue2)
        {
            return await databaseController.UpdateCmd(tableName, keyName1, keyName2, _keyValue1, _keyValue2);
        }
        protected async Task<T2> SelectCmd(T1 _keyValue1)
        {
            return await databaseController.SelectCmd<T1, T2>(tableName, keyName1, keyName2, _keyValue1);
        }
    }

    public abstract class ADatabaseTableSO : ScriptableObject 
    {
        protected DatabaseController databaseController;
        protected abstract string tableName { get;}
        protected abstract string keyName1 { get; }

        public void Init(DatabaseController _databaseController)
        {
            databaseController = _databaseController;
        }

        public async Task<long> SelectCount<T1>(T1 _keyValue1)
        {
            return await databaseController.SelectCount(tableName, keyName1, _keyValue1);
        }

        public async Task<long> SelectTotalCount()
        {
            return await databaseController.SelectTotalCount(tableName);
        }

        public void TruncateTable()
        {
            databaseController.TruncateTable(tableName);
        }
    }
}
