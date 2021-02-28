using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        InputController.ElevatorMode(true);
    }

    private void OnTriggerExit(Collider collider)
    {
        InputController.ElevatorMode(false);
    }
}
