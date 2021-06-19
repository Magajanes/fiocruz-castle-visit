using UnityEngine;
using UnityEngine.UI;

public class SelectionMenu : MonoBehaviour
{
    [SerializeField]
    private Button[] _menuButtons;

    private void Awake()
    {
        MenuController.OnSelectionMenuFade += SetButtonsInteractive;
    }

    private void OnDestroy()
    {
        MenuController.OnSelectionMenuFade -= SetButtonsInteractive;
    }

    public void SetButtonsInteractive(bool isInteractive)
    {
        foreach (Button button in _menuButtons)
        {
            button.interactable = isInteractive;
        }
    }
}
