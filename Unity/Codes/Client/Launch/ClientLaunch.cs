using UnityEngine;

namespace TheMazeRunner
{
    public static class ClientLaunch
    {
        public static void Awake()
        {
            Debug.Log("Client Awake");
            AutoLinkTest test = new AutoLinkTest();
            test.Awake();
        }
    }
}
