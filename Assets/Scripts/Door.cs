using UnityEngine;

public class Door : MonoBehaviour
{
    public const string OPEN_DOOR_TRIGGER_NAME = "Open";
    
    [SerializeField]
    private Animator animator;

    private bool _isOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_isOpen)
            return;

        InputController.OnDoorInteractionPress += OpenDoor;
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isOpen)
            return;
        
        InputController.OnDoorInteractionPress -= OpenDoor;
    }

    private void OpenDoor()
    {
        if (_isOpen)
            return;
        
        animator.SetTrigger(OPEN_DOOR_TRIGGER_NAME);
        _isOpen = true;
        InputController.OnDoorInteractionPress -= OpenDoor;
    }
}
