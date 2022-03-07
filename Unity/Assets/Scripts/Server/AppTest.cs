using MessagePack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace TheMazeRunner
{
    public class AppTest : MonoBehaviour
    {
        [SerializeField]
        private DatabaseTableRankSO tableRankSO;
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.I))
            {
                InsertTest();
            }
            if (Input.GetKeyUp(KeyCode.U))
            {
                UpdateTest();
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                SelectTest();
            }
        }

        private async void InsertTest()
        {
            RankData _rankData = new RankData();
            _rankData.RankID = 1001;
            _rankData.PreUpdateTime = 1000;
            _rankData.RankList = new List<long>();
            _rankData.RankList.Add(10001);
            await tableRankSO.InsertIntoDatabase(_rankData);
        }

        private void UpdateTest()
        {
            RankData _rankData = new RankData();
            _rankData.RankID = 1001;
            _rankData.PreUpdateTime = 1000;
            _rankData.RankList = new List<long>();
            _rankData.RankList.Add(10002);
            tableRankSO.UpdateDatabase(_rankData).Coroutine();
        }

        private async void SelectTest()
        {
            //RankData _rankData = await tableRankSO.SelectDatabase(1001);
            tableRankSO.AddListener<DatabaseTableRankSO, string>(nameof(tableRankSO.TestXXX), OnTestXXXUpdate);
            tableRankSO.SetValueBoradCast(nameof(tableRankSO.TestXXX), "hahahah");
            tableRankSO.RemoveListener<DatabaseTableRankSO, string>(nameof(tableRankSO.TestXXX), OnTestXXXUpdate);

            //button3_Click(null, null);
        }

        private void OnTestXXXUpdate(string _xxx)
        {
            Debug.Log($"{_xxx}");
        }

        private Action OnTest;

        private void button3_Click(object sender, EventArgs e)
        { 
            var Instance = Activator.CreateInstance(typeof(AppTest)) as AppTest;
            MethodInfo methodInfo = typeof(AppTest).GetMethod("Method1");


            Action dd = Delegate.CreateDelegate(typeof(Action), Instance, methodInfo) as Action;

            OnTest?.Invoke();

            //Instance.GetType().GetMethod("Method1").GetMethodBody() = Instance.GetType().GetMethod("Method2");


            OnTest += Instance.GetType().GetMethod("Method1").CreateDelegate(typeof(Action), Instance) as Action;
            //OnTest += Instance.GetType().GetMethod("Method2").CreateDelegate(typeof(Action), Instance) as Action;
            //OnTest?.Invoke();

            Method1();
        }
        public void Method1()
        {
            Debug.Log(1111);
        }

        public void Method2()
        {
            Debug.Log(2222);
        }

        public Delegate CreateDelegateFromMethodInfo<T>(object instance, MethodInfo method) where T : EventArgs//约束泛型T只能是来自EventArgs类型的    
        {
            Delegate del = Delegate.CreateDelegate(typeof(EventHandler<T>), instance, method);
            EventHandler<T> mymethod = del as EventHandler<T>;
            return mymethod;
        }
    }
}
