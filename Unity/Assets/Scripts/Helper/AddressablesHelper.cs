using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class AddressablesHelper
{
    public static async Task<TObject> LoadAssetAsync<TObject>(string address)
    {
        return await Addressables.LoadAssetAsync<TObject>(address).Task;
    }
}
