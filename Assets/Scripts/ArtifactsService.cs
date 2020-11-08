using System;
using System.Collections.Generic;
using UnityEngine;

public static class ArtifactsService
{
    private static Dictionary<int, string> _artifactInfoPathById = new Dictionary<int, string>()
    {
        { 1, "Artifacts/Cylinder" },
        { 4, "Artifacts/Sphere" }
    };

    private static Dictionary<int, ArtifactInfo> _artifactInfoById;

    public static void LoadArtifactsInfo(Action onAssetsLoaded = null)
    {
        if (_artifactInfoById != null)
            return;

        _artifactInfoById = new Dictionary<int, ArtifactInfo>();
        foreach (KeyValuePair<int, string> pair in _artifactInfoPathById)
        {
            var request = Resources.LoadAsync(pair.Value);
            request.completed += (operation) =>
            {
                var artifactInfo = request.asset as ArtifactInfo;
                _artifactInfoById.Add(pair.Key, artifactInfo);
            };
        }

        onAssetsLoaded?.Invoke();
    }

    public static bool TryGetArtifactInfoById(int id, ref ArtifactInfo artifactInfo)
    {
        if (!_artifactInfoById.ContainsKey(id))
        {
            Debug.Log($"Artifact with id: {id} not found!");
            return false;
        }
        artifactInfo = _artifactInfoById[id];
        return true;
    }
}
