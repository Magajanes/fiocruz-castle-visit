using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        InputController.Instance.ElevatorMode(true);
    }

    private void OnTriggerExit(Collider collider)
    {
        InputController.Instance.ElevatorMode(false);
    }
}
