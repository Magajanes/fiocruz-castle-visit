using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    private Button _backButton;
    [SerializeField]
    private Toggle _invertYToggle;
    
    public void Initialize()
    {
        PlayerPrefsService.ApplySavedPlayerPrefs(out bool invertY);
        _invertYToggle.isOn = invertY;
    }

    public void SetInvertMouseY(bool active)
    {
        PlayerPrefsService.SetBool(PlayerPrefsService.INVERT_MOUSE_Y, active);
        InputController.Instance.SetInvertMouseY(active);
    }
}
