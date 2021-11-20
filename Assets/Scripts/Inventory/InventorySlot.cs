using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public const string ICON_IMAGE_PATH = "Sprites/Icons/{0}";
    
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

        if (_artifactInfo == null) return;

        artifactImage.color = collectedColor;
        ArtifactSpriteHelper.LoadArtifactSprite(
            string.Format(ICON_IMAGE_PATH, _artifactInfo.ImageName),
            (sprite) => artifactImage.sprite = sprite
        );
    }

    public void OnSlotClick()
    {
        if (_artifactInfo == null) return;

        MenuController.Instance.ShowArtifactInfo(_artifactInfo.Id);
    }
}
