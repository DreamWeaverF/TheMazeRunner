using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TheMazeRunner
{
    public enum LaunchType
    {
        None,
        Dev,
        Release_Clent,
        Release_Server,
    }

    public class Launch : MonoBehaviour
    {
        // Start is called before the first frame update
        public async void Start()
        {
            TextAsset clientDll = await AddressablesHelper.LoadAssetAsync<TextAsset>("Client.dll");
            TextAsset clientpdb = await AddressablesHelper.LoadAssetAsync<TextAsset>("Client.pdb");

            Assembly assembly = Assembly.Load(clientDll.bytes, clientpdb.bytes);
            MethodInfo methodInfo = assembly.GetType("Client").GetMethod("Start");
            methodInfo.Invoke(null, new object[0]);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

