using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

public static class AddressablesEditor
{
    public static AddressableAssetEntry GetAddressableAssetEntry(UnityEngine.Object o)
    {
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        AddressableAssetEntry entry = null;

        bool foundAsset = AssetDatabase.TryGetGUIDAndLocalFileIdentifier(o, out string guid, out long _);
        if (foundAsset && settings)
        {
            entry = settings.FindAssetEntry(guid);
        }

        return entry;
    }

    public static void ModifyAddressableAssetEntry(UnityEngine.Object o, string group, string address)
    {
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        AddressableAssetGroup parentGroup = settings.FindGroup(group);

        bool foundAsset = AssetDatabase.TryGetGUIDAndLocalFileIdentifier(o, out string guid, out long _);
        if (foundAsset)
        {
            var entry = settings.CreateOrMoveEntry(guid, parentGroup);
            entry.address = address;
        }
    }
}
