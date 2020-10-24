using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactInfoPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI description;
    [SerializeField]
    private Image image;

    private ArtifactInfo _currentInfo;
    public static event Action<ArtifactInfo> OnCollect;

    public void Initialize(ArtifactInfo info)
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
