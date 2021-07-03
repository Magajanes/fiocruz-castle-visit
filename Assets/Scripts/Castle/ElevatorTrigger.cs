using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        InputController.Instance.ElevatorMode(true);
        TutorialController.Instance.ShowTutorial(TutorialSubject.ElevatorMove);
    }

    private void OnTriggerExit(Collider collider)
    {
        InputController.Instance.ElevatorMode(false);
        TutorialController.Instance.HideTutorial(TutorialSubject.ElevatorMove);
    }
}
