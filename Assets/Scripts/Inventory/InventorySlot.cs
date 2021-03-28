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

    private ArtifactInfo _artifactInfo;

    public void Initialize(int artifactId)
    {
        _artifactInfo = ArtifactsService.GetArtifactInfoById(artifactId);

        if (_artifactInfo == null)
            return;

        artifactImage.color = collectedColor;
        ArtifactSpriteHelper.LoadArtifactSprite(
            _artifactInfo.ImagePath,
            (sprite) => artifactImage.sprite = sprite
        );
    }

    public void ShowArtifactInfo()
    {
        if (_artifactInfo == null)
            return;

        var args = UIPanel.InitArgs.Create(_artifactInfo.Id, UIState.InventoryPanel);
        UIManager.ChangeState(UIState.ArtifactInfo, args);
    }
}
