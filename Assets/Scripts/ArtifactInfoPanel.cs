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

    public void Initialize(ArtifactInfo info)
    {
        title.text = info.Name;
        description.text = info.Description;
        image.sprite = info.Image;
    }
}
