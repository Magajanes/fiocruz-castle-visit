using UnityEngine;

public class Door : MonoBehaviour
{
    public const string OPEN_DOOR_TRIGGER_NAME = "Open";
    
    [SerializeField]
    private Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        InputController.OnDoorInteractionPress += OpenDoor;
    }

    private void OnTriggerExit(Collider other)
    {
        InputController.OnDoorInteractionPress -= OpenDoor;
    }

    private void OpenDoor()
    {
        animator.SetTrigger(OPEN_DOOR_TRIGGER_NAME);
    }
}
