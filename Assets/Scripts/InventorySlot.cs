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

    private int _artifactId;

    public void Initialize(int artifactId)
    {
        _artifactId = artifactId;
        artifactImage.color = collectedColor;
        if (ArtifactsService.TryGetArtifactSprite(artifactId, out Sprite sprite))
        {
            artifactImage.sprite = sprite;
        }
    }

    public void ShowArtifactInfo()
    {
        if (!ArtifactsService.ArtifactExists(_artifactId))
            return;

        var args = UIPanel.InitArgs.Create(_artifactId, UIState.InventoryPanel);
        UIManager.ChangeState(UIState.ArtifactInfo, args);
    }
}
