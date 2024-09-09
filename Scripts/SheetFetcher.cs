using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class SheetFetcher : ScriptableObject
{
    [HideInInspector] public string SheetsID;
    [HideInInspector] public string SheetName;
    [HideInInspector] public string Data { get; private set; }

    /// <summary>
    /// Fetched data parsed into a two-dimensional string array
    /// </summary>
    public string[][] ParsedData
    {
        get
        {
            if (string.IsNullOrEmpty(Data))
            {
                Debug.LogError("Data is null or empty");
                return null;
            }

            var rows = Data.Split(new char[] { '\r', '\n' });
            var result = new string[rows.Length][];

            for (int i = 0; i < rows.Length; i++)
            {
                var columns = rows[i].Split(",");
                result[i] = new string[columns.Length];
                Array.Copy(columns, result[i], columns.Length);
            }

            return result;
        }
    }

    /// <summary>
    /// Event triggered after data is successfully fetched. The fetched data is passed as a string parameter.
    /// </summary>
    public event Action<string> OnComplete;

    private void OnValidate()
    {
        SheetManager.AddDownloader(this);
    }

    public void Fetch()
    {
        string downloadLink = $"https://docs.google.com/spreadsheets/d/{SheetsID}/gviz/tq?tqx=out:csv&sheet={SheetName}";

        UnityWebRequest webRequest = UnityWebRequest.Get(downloadLink);

        var asyncOperation = webRequest.SendWebRequest();
        asyncOperation.completed += (x) =>
        {
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                string errorText = $"{name} web request failed:   {webRequest.error}";
                if (webRequest.error.Contains("Unauthorized")) errorText += "\nMake sure your google sheet is public!";
                Debug.LogError(errorText);

                webRequest.Dispose();
                return;
            }

            Data = webRequest.downloadHandler.text.Replace("\"", "");
            OnComplete?.Invoke(Data);
            HandleFetchedData(Data);
            webRequest.Dispose();
        };

        #if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        #endif
    }

    /// <summary>
    /// Override this method to handle the successfully fetched data.
    /// </summary>
    /// <param name="data">Fetched data.</param>
    public virtual void HandleFetchedData(string data)
    {

    }

    /// <param name="target">Value to search for.</param>
    /// <returns>Row of data where the target matches the value of the first column.</returns>
    public string[] GetRowByFirstColumn(string target)
    {
        return Array.Find(ParsedData, x => x[0] == target);
    }
}
