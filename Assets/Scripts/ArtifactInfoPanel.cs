using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIPanel : MonoBehaviour
{
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public abstract void Initialize(object context = null);
}

public class ArtifactInfoPanel : UIPanel
{
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI description;
    [SerializeField]
    private Image image;

    public override void Initialize(object context = null)
    {
        int artifactId = (int)context;
        ArtifactInfo info = null;
        if (ArtifactsService.TryGetArtifactInfoById(artifactId, ref info))
        {
            title.text = info.Name;
            description.text = info.Description;
            image.sprite = info.Image;
        }
    }
}
