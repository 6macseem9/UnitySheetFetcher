using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class SheetManager
{
    private static List<SheetFetcher> _fetchers = new List<SheetFetcher>();

    public static void AddDownloader(SheetFetcher downloader)
    {
        if (_fetchers.Contains(downloader)) return;
        _fetchers.Add(downloader);
    }

    #if UNITY_EDITOR
    [MenuItem("SheetFetcher/Fetch selected", false, 1)]
    public static void FetchSelected()
    {
        var guids = Selection.assetGUIDs;
        if (guids.Length == 0) 
        {
            Debug.LogWarning("Fetch failed: no fetcher selected");
            return; 
        }

        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            SheetFetcher downloader = AssetDatabase.LoadAssetAtPath<SheetFetcher>(path);
            downloader.Fetch();
        }
    }

    [MenuItem("SheetFetcher/Fetch All", false, 2)]
    #endif

    public static void FetchAll()
    {
        if(_fetchers.Count==0)
        {
            Debug.LogWarning("Fetch failed: list is empty");
            return;
        }

        foreach (SheetFetcher downloader in _fetchers)
            downloader.Fetch();
    }
}
