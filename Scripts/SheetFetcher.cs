using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName ="SO",menuName ="New so",order = 0)]
public class SheetFetcher : ScriptableObject
{
    [HideInInspector] public string SheetsID;
    [HideInInspector] public string SheetName;
    [HideInInspector] public string Data;

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

            Data = webRequest.downloadHandler.text;
            OnComplete(Data);
            webRequest.Dispose();
        };
    }

    /// <summary>
    /// Called after the data was successfully fetched.
    /// </summary>
    /// <param name="data"></param>
    public virtual void OnComplete(string data)
    {

    }

    /// <param name="data">Fetched data.</param>
    /// <returns>Two dimensional list of all rows and columns.</returns>
    public List<List<string>> ParseData(string data)
    {
        var result = new List<List<string>>();

        data = data.Replace("\"", "");
        var rows = data.Split(new char[] { '\r','\n'});
        for(int i = 0; i < rows.Length; i++)
        {
            result.Add(new List<string>());
            result[i].AddRange(rows[i].Split(","));
        }

        return result;
    }

    /// <param name="data">Fetched data.</param>
    /// <param name="target">Value to search for.</param>
    /// <returns>Row of data where the target matches the value of the first column.</returns>
    public List<string> GetRowByFirstColumn(string data, string target)
    {
        var parsedData = ParseData(data);
        return parsedData.First(x => x[0] == target); 
    }
}
