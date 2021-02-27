using System;
using System.Collections;
using UnityEngine;

public class RegionDoor : Door
{
    public const string CLOSE_DOOR_TRIGGER_NAME = "Close";
    
    [SerializeField]
    private int regionId;

    public static event Action<int> OnRegionEnter;

    protected override void OpenDoor()
    {
        base.OpenDoor();
        OnRegionEnter?.Invoke(regionId);
    }

    public void CloseDoor(Action onDoorsClosed = null)
    {
        if (!_isOpen) 
        {
            Debug.Log("Here!");
            return;
        }

        animator.SetTrigger(CLOSE_DOOR_TRIGGER_NAME);
        _isOpen = false;

        if (onDoorsClosed != null)
            StartCoroutine(ExecuteAfterOneSecond(onDoorsClosed));

        IEnumerator ExecuteAfterOneSecond(Action action)
        {
            yield return new WaitForSeconds(1);
            action.Invoke();
        }
    }
}
