﻿using UnityEngine;

public class ElevatorDoor : MonoBehaviour
{
    [SerializeField]
    private int floorNumber;
    [SerializeField]
    private Elevator elevator;

    private void CallElevator()
    {
        elevator.CallToFloor(floorNumber);
    }

    private void OnTriggerEnter(Collider collider)
    {
        InputController.Instance.ElevatorMode(true);
        InputController.OnElevatorCall += CallElevator;
        Debug.Log("Please call elevator!");
    }

    private void OnTriggerExit(Collider other)
    {
        InputController.Instance.ElevatorMode(false);
        InputController.OnElevatorCall -= CallElevator;
        Debug.Log("Nevermind!");
    }
}
