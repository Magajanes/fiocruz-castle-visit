using UnityEngine;
using TMPro;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _tutorialMessage;
    [SerializeField]
    private CanvasGroup _canvasGroup;

    public void SetMessage(string message)
    {
        _tutorialMessage.text = message;
    }

    public void Close()
    {
        _canvasGroup.alpha = 0;
    }
}
