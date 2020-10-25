using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIPanel : MonoBehaviour
{
    public abstract void Initialize(ArtifactInfo info);
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
    public static event Action<ArtifactInfo> OnCollect;

    public override void Initialize(ArtifactInfo info)
    {
        InputController.OnCollectButtonPress += Collect;
        
        if (_currentInfo != null && _currentInfo.Name == info.name)
            return;
        
        _currentInfo = info;

        title.text = _currentInfo.Name;
        description.text = _currentInfo.Description;
        image.sprite = _currentInfo.Image;
    }

    public void Collect()
    {
        var args = new InteractionArgs(UIState.Inactive, _currentInfo);
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
