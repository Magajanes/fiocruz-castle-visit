using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector3 _movementX;
    private Vector3 _movementY;
    private Vector3 _moveDirection;

    [Header("Parameters")]
    [SerializeField]
    private float speed;

    [Header("References")]
    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private Transform headTransform;

    private void Awake()
    {
        InputController.OnMoveInput += MoveTowards;
        InputController.OnTurnInput += TurnHeadTowards;
    }

    private void OnDestroy()
    {
        InputController.OnMoveInput -= MoveTowards;
        InputController.OnTurnInput -= TurnHeadTowards;
    }

    private void MoveTowards(Vector2 inputDirection)    
    {
        _movementX = inputDirection.x * transform.right;
        _movementY = inputDirection.y * transform.forward;
        _moveDirection = _movementX + _movementY;
        characterController.Move(_moveDirection * speed * Time.deltaTime);
    }

    private void TurnHeadTowards(Vector2 mouseInput)
    {
        transform.rotation = Quaternion.Euler(0, mouseInput.x, 0);
        headTransform.localRotation = Quaternion.Euler(mouseInput.y, 0, 0);
    }
}
