using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIPanel : MonoBehaviour
{
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public abstract void Initialize(InitArgs args);

    public class InitArgs
    {
        public int ArtifactId;

        public static InitArgs CreateWithId(int id)
        {
            return new InitArgs() { ArtifactId = id };
        }
    }
}

public class ArtifactInfoPanel : UIPanel
{
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI description;
    [SerializeField]
    private Image image;

    public override void Initialize(InitArgs args)
    {
        int artifactId = args.ArtifactId;
        ArtifactInfo artifactInfo = new ArtifactInfo();
        if (ArtifactsService.TryGetArtifactInfo(artifactId, ref artifactInfo))
        {
            SetPanel(artifactInfo);
        }
    }

    private void SetPanel(ArtifactInfo artifactInfo)
    {
        title.text = artifactInfo.Name;
        description.text = artifactInfo.Description;
        ArtifactsService.LoadArtifactSprite(
            artifactInfo.ImagePath,
            (sprite) =>
            {
                image.sprite = sprite;
            }
        );
    }
}
