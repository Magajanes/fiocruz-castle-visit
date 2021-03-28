using System;
using UnityEngine;

public static class ArtifactSpriteHelper
{
    public static void LoadArtifactSprite(string imagePath, Action<Sprite> onSpriteLoaded)
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
        var rect = new Rect(0, 0, texture.width, texture.height);
        return Sprite.Create(
            texture,
            rect,
            Vector2.one * 0.5f
        );
    }
}
