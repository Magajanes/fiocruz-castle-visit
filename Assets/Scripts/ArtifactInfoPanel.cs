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

    private ArtifactInfo _currentInfo;

    public override void Initialize(InitArgs args)
    {
        int artifactId = args.ArtifactId;
        if (ArtifactsService.TryGetArtifactInfo(artifactId, out ArtifactInfo artifactInfo))
        {
            _currentInfo = artifactInfo;
            SetPanel();
        }
    }

    private void SetPanel()
    {
        title.text = _currentInfo.Name;
        description.text = _currentInfo.Description;
        if (ArtifactsService.TryGetArtifactSprite(_currentInfo.Id, out Sprite sprite))
        {
            image.sprite = sprite;
        }
    }

    public void Collect()
    {
        InventoryService.SaveArtifact(_currentInfo.Id);
        UIManager.ChangeState(UIState.Inactive);
    }
}
