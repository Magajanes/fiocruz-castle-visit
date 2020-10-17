using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector3 _moveDirection;

    [Header("Parameters")]
    [SerializeField]
    private float speed;

    [Header("References")]
    [SerializeField]
    private InputController inputController;
    [SerializeField]
    private CharacterController charController;
    [SerializeField]
    private Transform headTransform;

    private void Awake()
    {
        inputController.OnMoveInput += MoveTowards;
        inputController.OnMouseInput += TurnHeadTowards;
    }

    private void OnDestroy()
    {
        inputController.OnMoveInput -= MoveTowards;
        inputController.OnMouseInput -= TurnHeadTowards;
    }

    private void MoveTowards(Vector3 inputDirection)
    {
        _moveDirection = inputDirection.magnitude > 1 ? inputDirection.normalized : inputDirection;
        charController.Move(_moveDirection * speed * Time.deltaTime);
    }

    private void TurnHeadTowards(Vector2 mouseInput)
    {
        transform.Rotate(Vector3.up, mouseInput.x);
        headTransform.Rotate(Vector3.right, mouseInput.y);

        Debug.Log(headTransform.rotation.eulerAngles.x);
    }
}
