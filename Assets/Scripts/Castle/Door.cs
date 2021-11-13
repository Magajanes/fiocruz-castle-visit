using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    public const string OPEN_DOOR_TRIGGER_NAME = "Open";
    
    [SerializeField]
    protected Animator animator;

    protected bool _isOpen = false;

    public static event Action OnDoorOpen;

    private void OnTriggerEnter(Collider other)
    {
        if (_isOpen)
            return;

        TutorialController.Instance.ShowTutorial(TutorialSubject.Door);
        InputController.OnDoorInteractionPress -= OpenDoor;
        InputController.OnDoorInteractionPress += OpenDoor;
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isOpen)
            return;

        TutorialController.Instance.HideTutorial(TutorialSubject.Door);
        InputController.OnDoorInteractionPress -= OpenDoor;
    }

    protected virtual void OpenDoor()
    {
        if (_isOpen)
            return;

        _isOpen = true;
        animator.SetTrigger(OPEN_DOOR_TRIGGER_NAME);
        
        OnDoorOpen?.Invoke();
        InputController.OnDoorInteractionPress -= OpenDoor;
    }

    private void OnDestroy()
    {
        InputController.OnDoorInteractionPress -= OpenDoor;
    }
}
