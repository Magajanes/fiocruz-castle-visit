using UnityEngine;

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

    private void OnDestroy()
    {
        InputController.OnElevatorCall -= CallElevator;
    }

    private void OnTriggerEnter(Collider collider)
    {
        InputController.OnElevatorCall -= CallElevator;
        InputController.OnElevatorCall += CallElevator;

        InputController.Instance.ElevatorMode(true);
        TutorialController.Instance.ShowTutorial(TutorialSubject.ElevatorCall);
    }

    private void OnTriggerExit(Collider other)
    {
        InputController.OnElevatorCall -= CallElevator;

        InputController.Instance.ElevatorMode(false);
        TutorialController.Instance.HideTutorial(TutorialSubject.ElevatorCall);
    }
}
