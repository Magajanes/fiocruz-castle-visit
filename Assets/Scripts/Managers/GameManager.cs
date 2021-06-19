using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        InputController.Instance.LockInputs(false);
        InputController.Instance.SetInputScheme(UIState.Inactive);
    }
}
