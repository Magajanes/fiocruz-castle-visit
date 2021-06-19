using System;
using System.Collections.Generic;
using UnityEngine;

public static class ArtifactsService
{
    public const string GAME_ARTIFACTS_ASSET_PATH = "Artifacts/GameArtifacts";
    private static Dictionary<int, ArtifactInfo> _artifactInfoById;

    public static void LoadGameArtifacts(Action onAssetsLoaded = null)
    {
        if (_artifactInfoById != null)
            return;

        _artifactInfoById = new Dictionary<int, ArtifactInfo>();

        ResourceRequest request = Resources.LoadAsync(GAME_ARTIFACTS_ASSET_PATH);
        request.completed += (operation) =>
        {
            var gameArtifacts = request.asset as GameArtifacts;
            foreach (var artifact in gameArtifacts.Artifacts)
            {
                _artifactInfoById[artifact.Id] = artifact;
            }
            onAssetsLoaded?.Invoke();
        };
    }

    public static bool ArtifactExists(int id)
    {
        if (!_artifactInfoById.ContainsKey(id))
        {
            Debug.Log($"Artifact with id: {id} does not exist!");
            return false;
        }
        return true;
    }

    public static ArtifactInfo GetArtifactInfoById(int id)
    {
        if (!ArtifactExists(id))
            return null;

        return _artifactInfoById[id];
    }
}
