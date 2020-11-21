using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField]
    private Color notCollectedColor;
    [SerializeField]
    private Color collectedColor = Color.white;
    [Header("References")]
    [SerializeField]
    private Image artifactImage;

    public void Initialize(int artifactId)
    {
        artifactImage.color = collectedColor;
        if (ArtifactsService.TryGetArtifactSprite(artifactId, out Sprite sprite))
        {
            artifactImage.sprite = sprite;
        }
    }
}
