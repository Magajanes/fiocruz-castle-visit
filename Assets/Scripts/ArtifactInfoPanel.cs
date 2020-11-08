using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIPanel : MonoBehaviour
{
    public abstract void Show(int artifactId = 0);
}

public class ArtifactInfoPanel : UIPanel
{
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI description;
    [SerializeField]
    private Image image;

    private ArtifactsService _artifactsService;
    private ArtifactInfo _currentInfo;
    public static event Action<ArtifactInfo> OnCollect;

    public void Initialize(ArtifactsService artifactsService)
    {
        _artifactsService = artifactsService;
    }

    public override void Show(int artifactId)
    {
        InputController.OnCollectButtonPress += Collect;
        ArtifactInfo artifactInfo = _artifactsService.GetArtifactInfoById(artifactId);

        if (_currentInfo != null && _currentInfo.Name == artifactInfo.name)
            return;
        
        _currentInfo = artifactInfo;
        title.text = _currentInfo.Name;
        description.text = _currentInfo.Description;
        image.sprite = _currentInfo.Image;
    }

    public void Collect()
    {
        var args = new InteractionArgs(UIState.Inactive, _currentInfo.Id);
        UIManager.ChangeUIState(args);
        OnCollect?.Invoke(_currentInfo);
    }

    private void OnDisable()
    {
        InputController.OnCollectButtonPress -= Collect;
    }

    private void OnDestroy()
    {
        InputController.OnCollectButtonPress -= Collect;
    }
}
