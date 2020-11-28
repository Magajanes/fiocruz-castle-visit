using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class PlayerPrefsService
{
    public static void DeleteInventory()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void SaveCollectedArtifacts(List<int> collectedArtifacts)
    {
        if (collectedArtifacts == null)
        {
            Debug.LogError("Trying to save a null List in Collected Artifacts!");
            return;
        }

        string artifactsJson = JsonConvert.SerializeObject(collectedArtifacts);
        PlayerPrefs.SetString("collectedArtifacts", artifactsJson);
    }

    public static List<int> LoadCollectedArtifactsIds()
    {
        var collectedArtifacts = new List<int>();
        if (PlayerPrefs.HasKey("collectedArtifacts"))
        {
            string artifactsJson = PlayerPrefs.GetString("collectedArtifacts");
            collectedArtifacts = JsonConvert.DeserializeObject<List<int>>(artifactsJson);
        }
        return collectedArtifacts;
    }
}
