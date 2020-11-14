using System;
using System.Collections.Generic;
using UnityEngine;

public static class ArtifactsService
{
    private const string GAME_ARTIFACTS_ASSET_PATH = "Artifacts/GameArtifacts";
    private static Dictionary<int, ArtifactInfo> _artifactInfoById;

    public static bool ArtifactExists(int id)
    {
        if (!_artifactInfoById.ContainsKey(id))
        {
            Debug.Log($"Artifact with id: {id} does not exist!");
            return false;
        }
        return true;
    }

    public static void LoadGameArtifacts(Action onAssetsLoaded)
    {
        if (_artifactInfoById != null)
            return;

        _artifactInfoById = new Dictionary<int, ArtifactInfo>();

        var request = Resources.LoadAsync(GAME_ARTIFACTS_ASSET_PATH);
        request.completed += (operation) => 
        {
            var gameArtifacts = request.asset as GameArtifacts;
            foreach (var artifact in gameArtifacts.Artifacts)
            {
                _artifactInfoById.Add(artifact.Id, artifact);
            }
            onAssetsLoaded?.Invoke();
        };
    }

    public static bool TryGetArtifactInfo(int id, ref ArtifactInfo artifactInfo)
    {
        if (!ArtifactExists(id))
            return false;

        artifactInfo = _artifactInfoById[id];
        return true;
    }

    public static bool TryGetArtifactName(int id, ref string name)
    {
        if (!ArtifactExists(id))
            return false;

        name = _artifactInfoById[id].Name;
        return true;
    }

    public static bool TryGetArtifactDescription(int id, ref string description)
    {
        if (!ArtifactExists(id))
            return false;

        description = _artifactInfoById[id].Description;
        return true;
    }

    public static bool TryGetArtifactImagePath(int id, ref string imagePath)
    {
        if (!ArtifactExists(id))
            return false;

        imagePath = _artifactInfoById[id].ImagePath;
        return true;
    }

    public static void TryLoadArtifactSprite(int id, Action<Sprite> onSpriteLoaded)
    {
        if (!ArtifactExists(id))
            return;

        LoadArtifactSprite(_artifactInfoById[id].ImagePath, onSpriteLoaded);
    }

    public static void LoadArtifactSprite(string path, Action<Sprite> onSpriteLoaded)
    {
        var request = Resources.LoadAsync(path);
        request.completed += (operation) =>
        {
            //TODO: Convert texture to sprite or use RawImage component!
            var texture = request.asset as Texture2D;
            var sprite = Sprite.Create(texture, Rect.zero, Vector2.zero);
            onSpriteLoaded?.Invoke(sprite);
        };
    }
}
