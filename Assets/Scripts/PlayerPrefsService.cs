using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class PlayerPrefsService
{
    public static void SaveCollectedArtifacts(List<int> collectedArtifacts)
    {
        if (collectedArtifacts == null)
        {
            Debug.LogError("Trying to save a null dictionary in Collected Artifacts!");
            return;
        }

        var artifactsJson = JsonConvert.SerializeObject(collectedArtifacts);
        PlayerPrefs.SetString("collectedArtifacts", artifactsJson);
    }

    public static List<int> LoadCollectedArtifactsIds()
    {
        var collectedArtifacts = new List<int>();
        if (PlayerPrefs.HasKey("collectedArtifacts"))
        {
            var artifactsJson = PlayerPrefs.GetString("collectedArtifacts");
            collectedArtifacts = JsonConvert.DeserializeObject<List<int>>(artifactsJson);
        }
        return collectedArtifacts;
    }

    public static void SaveInventoryEntry(InventoryEntry entry)
    {
        var entryJson = JsonConvert.SerializeObject(entry);
        PlayerPrefs.SetString($"entry_{entry.Id}", entryJson);
    }

    public static InventoryEntry LoadInventoryEntry(int id)
    {
        var entryKey = $"entry_{id}";
        if (PlayerPrefs.HasKey(entryKey))
        {
            var entryJson = PlayerPrefs.GetString(entryKey);
            return JsonConvert.DeserializeObject<InventoryEntry>(entryJson);
        }
        return null;
    }
}
