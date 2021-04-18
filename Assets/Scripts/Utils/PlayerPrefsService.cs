using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsService
{
    public const string COLLECTED_ARTIFACTS_PLAYERPREFS_KEY = "collectedArtifacts";
    public const string INVERT_MOUSE_Y = "invertMouseY";

    public static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }

    public static bool GetBool(string key)
    {
        if (!PlayerPrefs.HasKey(key))
            return false;
        
        int value = PlayerPrefs.GetInt(key);
        return value == 1;
    }

    public static Dictionary<int, bool> LoadCollectedArtifacts()
    {
        var collectedArtifacts = new Dictionary<int, bool>();

        if (!PlayerPrefs.HasKey(COLLECTED_ARTIFACTS_PLAYERPREFS_KEY))
        {
            var request = Resources.LoadAsync(ArtifactsService.GAME_ARTIFACTS_ASSET_PATH);
            request.completed += (operation) =>
            {
                var gameArtifacts = request.asset as GameArtifacts;
                foreach (ArtifactInfo artifact in gameArtifacts.Artifacts)
                {
                    collectedArtifacts[artifact.Id] = false;
                }
                SaveCollectedArtifacts(collectedArtifacts);
            };
        }
        else
        {
            string artifactsJson = PlayerPrefs.GetString(COLLECTED_ARTIFACTS_PLAYERPREFS_KEY);
            collectedArtifacts = JsonConvert.DeserializeObject<Dictionary<int, bool>>(artifactsJson);
            Debug.Log($"Loaded serialized dictionary: {artifactsJson}");
        }

        return collectedArtifacts;
    }

    public static void SaveCollectedArtifacts(Dictionary<int, bool> collectedArtifacts)
    {
        string artifactsJson = JsonConvert.SerializeObject(collectedArtifacts);
        PlayerPrefs.SetString(COLLECTED_ARTIFACTS_PLAYERPREFS_KEY, artifactsJson);
        Debug.Log($"Saved serialized dictionary: {artifactsJson}");
    }
}
