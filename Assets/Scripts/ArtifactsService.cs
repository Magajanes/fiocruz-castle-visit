using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public static class ArtifactsService
{
    private const string GAME_ARTIFACTS_ASSET_PATH = "Artifacts/GameArtifacts";
    private static Dictionary<int, ArtifactInfo> _artifactInfoById;
    private static Dictionary<int, Sprite> _artifactSpriteById;

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
        _artifactSpriteById = new Dictionary<int, Sprite>();

        var request = Resources.LoadAsync(GAME_ARTIFACTS_ASSET_PATH);
        request.completed += (operation) => 
        {
            var gameArtifacts = request.asset as GameArtifacts;
            foreach (var artifact in gameArtifacts.Artifacts)
            {
                _artifactInfoById.Add(artifact.Id, artifact);
                LoadArtifactSprite(artifact.ImagePath, (sprite) =>
                {
                    _artifactSpriteById.Add(artifact.Id, sprite);
                });
            }
            onAssetsLoaded?.Invoke();
        };
    }

    public static bool TryGetArtifactInfo(int id, out ArtifactInfo artifactInfo)
    {
        if (!ArtifactExists(id))
        {
            artifactInfo = null;
            return false;
        }

        artifactInfo = _artifactInfoById[id];
        return true;
    }

    public static bool TryGetArtifactName(int id, out string name)
    {
        if (!ArtifactExists(id))
        {
            name = string.Empty;
            return false;
        }

        name = _artifactInfoById[id].Name;
        return true;
    }

    public static bool TryGetArtifactDescription(int id, out string description)
    {
        if (!ArtifactExists(id))
        {
            description = string.Empty;
            return false;
        }

        description = _artifactInfoById[id].Description;
        return true;
    }

    public static bool TryGetArtifactSprite(int id, out Sprite sprite)
    {
        if (!ArtifactExists(id))
        {
            sprite = null;
            return false;
        }

        sprite = _artifactSpriteById[id];
        return true;
    }

    private static void LoadArtifactSprite(string imagePath, Action<Sprite> onSpriteLoaded)
    {
        var request = Resources.LoadAsync(imagePath);
        request.completed += (operation) =>
        {
            var texture = request.asset as Texture2D;
            var sprite = GetSpriteFromTexture(texture);
            onSpriteLoaded?.Invoke(sprite);
        };
    }

    private static Sprite GetSpriteFromTexture(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
    }
}
