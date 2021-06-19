using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public void SetInvertMouseY(bool active)
    {
        PlayerPrefsService.SetBool(PlayerPrefsService.INVERT_MOUSE_Y, active);
        InputController.Instance.SetInvertMouseY(active);
    }
}
