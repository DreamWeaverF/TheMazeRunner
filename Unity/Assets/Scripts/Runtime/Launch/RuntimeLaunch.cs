using System.IO;
using System.Reflection;
using UnityEngine;

namespace TheMazeRunner
{
    public class RuntimeLaunch : MonoBehaviour
    {
        public const string BuildOutputDir = "./Temp/Bin/Debug";
        //public const 

        void Start()
        {
            //Dictionary<string, UnityEngine.Object> dictionary = AssetsBundleHelper.LoadBundle("code.unity3d");
            //byte[] assBytes = ((TextAsset)dictionary["Code.dll"]).bytes;
            //byte[] pdbBytes = ((TextAsset)dictionary["Code.pdb"]).bytes;

            //assembly = Assembly.Load(assBytes, pdbBytes);


            //Dictionary<string, Type> types = AssemblyHelper.GetAssemblyTypes(typeof(Game).Assembly, this.assembly);
            //Game.EventSystem.Add(types);

            //IStaticMethod start = new MonoStaticMethod(assembly, "ET.Client.Entry", "Start");
            //start.Run();

            //TextAsset clientDll = Resources.Load<TextAsset>("Client.dll");
            //TextAsset clientPdb = Resources.Load<TextAsset>("Client.pdb");
            //Assembly assembly = Assembly.Load(clientDll.bytes, clientPdb.bytes);
            byte[] assBytes = File.ReadAllBytes(Path.Combine(BuildOutputDir, "ClientCode.dll"));
            byte[] pdbBytes = File.ReadAllBytes(Path.Combine(BuildOutputDir, "ClientCode.pdb"));
            Assembly assembly = Assembly.Load(assBytes, pdbBytes);
            MethodInfo method = assembly.GetType("TheMazeRunner.ClientLaunch").GetMethod("Awake");
            method.Invoke(null, new object[0]);
        }
    }
}
