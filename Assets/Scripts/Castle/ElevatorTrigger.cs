using System;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    public static event Action OnElevatorEnter;
    
    private void OnTriggerEnter(Collider collider)
    {
        InputController.Instance.ElevatorMode(true);
        OnElevatorEnter?.Invoke();
    }

    private void OnTriggerExit(Collider collider)
    {
        InputController.Instance.ElevatorMode(false);
    }
}
