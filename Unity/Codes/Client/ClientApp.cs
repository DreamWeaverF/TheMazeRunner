using UnityEngine;

namespace TheMazeRunner
{
    public static class ClientApp
    {
        public static void Awake()
        {
            LogHelper.Trace("Client Awake");
            AutoLinkTest test = new AutoLinkTest();
            test.Awake();
        }

        public static void Update()
        {

        }

        public static void Destory()
        {

        }
    }
}
