using System;
using UnityEngine;

public class ElevatorDoor : MonoBehaviour
{
    [SerializeField]
    private int floorNumber;
    [SerializeField]
    private Elevator elevator;

    public static event Action OnElevatorReached;

    private void CallElevator()
    {
        elevator.CallToFloor(floorNumber);
    }

    private void OnTriggerEnter(Collider collider)
    {
        InputController.OnElevatorCall += CallElevator;

        InputController.Instance.ElevatorMode(true);
        OnElevatorReached?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        InputController.OnElevatorCall -= CallElevator;

        InputController.Instance.ElevatorMode(false);
    }
}
