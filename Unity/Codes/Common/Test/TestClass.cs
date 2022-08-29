using System;
using System.Reflection;

namespace TheMazeRunner
{
    public class TestClass : Entiy
    {
        public int TestCount = 10;

        [FixedUpdateMethod]
        public void FixedUpdate()
        {
            LogHelper.Trace($"FixedUpdate {TestCount}");
            TestCount--;
        }

        [UpdateMethod]
        public void Update()
        {
            LogHelper.Trace("Update");
        }

        [SynchronizeMethod(ClassName = "TestClass",FieldName = "TestCount")]
        public void OnTestCountChange(int count)
        {
            LogHelper.Debug($"Count: {count}");
        }

        public void Clear()
        {
            LogHelper.Debug("Clear");
        }
    }
}
