using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerPrefsService : MonoBehaviour
{
    public const string COLLECTED_ARTIFACTS_PLAYERPREFS_KEY = "collectedArtifacts";

    public static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void LoadCollectedArtifacts(Dictionary<int, bool> collectedArtifacts)
    {
        collectedArtifacts.Clear();

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

            return;
        }

        string artifactsJson = PlayerPrefs.GetString(COLLECTED_ARTIFACTS_PLAYERPREFS_KEY);
        collectedArtifacts = JsonConvert.DeserializeObject<Dictionary<int, bool>>(artifactsJson);
        Debug.Log($"Loaded serialized dictionary: {artifactsJson}");
    }

    public static void SaveCollectedArtifacts(Dictionary<int, bool> collectedArtifacts)
    {
        string artifactsJson = JsonConvert.SerializeObject(collectedArtifacts);
        PlayerPrefs.SetString(COLLECTED_ARTIFACTS_PLAYERPREFS_KEY, artifactsJson);
        Debug.Log($"Saved serialized dictionary: {artifactsJson}");
    }
}
