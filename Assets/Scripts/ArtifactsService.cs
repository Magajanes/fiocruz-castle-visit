using System;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactsService
{
    private static Dictionary<int, string> _artifactInfoPathById = new Dictionary<int, string>()
    {
        { 1, "Artifacts/Cylinder" },
        { 4, "Artifacts/Sphere" }
    };

    private PlayerPrefsService _playerPrefsService;
    private Dictionary<int, ArtifactInfo> _artifactInfoById = new Dictionary<int, ArtifactInfo>();

    public ArtifactsService(PlayerPrefsService playerPrefsService)
    {
        _playerPrefsService = playerPrefsService;
    }

    public void LoadArtifacts(Action onArtifactsLoaded)
    {
        foreach (KeyValuePair<int, string> pair in _artifactInfoPathById)
        {
            ResourceRequest request = Resources.LoadAsync(pair.Value);
            request.completed += (operation) =>
            {
                var asset = request.asset as ArtifactInfo;
                _artifactInfoById.Add(asset.Id, asset);
            };
        }

        onArtifactsLoaded?.Invoke();
    }

    public ArtifactInfo GetArtifactInfoById(int id)
    {
        return _artifactInfoById[id];
    }
}
