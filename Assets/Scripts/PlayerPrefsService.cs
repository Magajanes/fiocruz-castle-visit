using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsService
{
    public static void SaveCollectedArtifacts(Dictionary<int, ArtifactInfo> collectedArtifacts)
    {
        if (collectedArtifacts == null)
        {
            Debug.LogError("Trying to save a null dictionary in Collected Artifacts!");
            return;
        }

        var artifactsJson = JsonUtility.ToJson(collectedArtifacts);
        PlayerPrefs.SetString("collectedArtifacts", artifactsJson);
    }

    public static Dictionary<int, ArtifactInfo> LoadCollectedArtifacts()
    {
        var collectedArtifacts = new Dictionary<int, ArtifactInfo>();
        if (PlayerPrefs.HasKey("collectedArtifacts"))
        {
            var artifactsJson = PlayerPrefs.GetString("collectedArtifacts");
            collectedArtifacts = JsonUtility.FromJson<Dictionary<int, ArtifactInfo>>(artifactsJson);
        }
        return collectedArtifacts;
    }
}
