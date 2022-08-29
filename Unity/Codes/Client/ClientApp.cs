using System;
using UnityEngine;

namespace TheMazeRunner
{
    public class ClientApp : Entiy
    {
        [BroadcastField]
        public float Aa;
        public void Awake()
        {
            LogHelper.Trace("Client Awake");
            AutoLinkTest test = new AutoLinkTest();
            test.Awake();
        }
        [UpdateMethod]
        public void Update()
        {

        }

        [FixedUpdateMethod]
        public void FixedUpdate()
        {

        }
        [SynchronizeMethod(ClassName = "ClientApp", FieldName = "Aa")]
        public void OnAaChangeValue(float aa)
        {
            LogHelper.Debug($"Aa_{aa}");
        }
    }
}
