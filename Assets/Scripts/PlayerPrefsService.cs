using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class PlayerPrefsService
{
    public void DeleteInventory()
    {
        PlayerPrefs.DeleteAll();
    }

    public void SaveCollectedArtifacts(List<int> collectedArtifacts)
    {
        if (collectedArtifacts == null)
        {
            Debug.LogError("Trying to save a null dictionary in Collected Artifacts!");
            return;
        }

        var artifactsJson = JsonConvert.SerializeObject(collectedArtifacts);
        PlayerPrefs.SetString("collectedArtifacts", artifactsJson);
    }

    public List<int> LoadCollectedArtifactsIds()
    {
        var collectedArtifacts = new List<int>();
        if (PlayerPrefs.HasKey("collectedArtifacts"))
        {
            var artifactsJson = PlayerPrefs.GetString("collectedArtifacts");
            collectedArtifacts = JsonConvert.DeserializeObject<List<int>>(artifactsJson);
        }
        return collectedArtifacts;
    }
}
