using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector3 _walkDirection;
    private Vector3 _strafeDirection;
    private Vector3 _verticalDirection;
    private Vector3 _moveDirection;

    [Header("Parameters")]
    [SerializeField]
    private float speed;
    [SerializeField]
    private float gravityModifier;

    [Header("References")]
    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private Transform headTransform;

    public static Movement Instance;
    private bool _lockMovement;

    private void Awake()
    {
        Instance = this;
        
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
        if (_lockMovement)
            return;
        
        _walkDirection = inputDirection.x * transform.right;
        _strafeDirection = inputDirection.y * transform.forward;
        CalculateGravity();
        _moveDirection = _walkDirection + _strafeDirection + _verticalDirection;
        characterController.Move(_moveDirection * speed * Time.deltaTime);
    }

    private void CalculateGravity()
    {
        _verticalDirection = _moveDirection.y * Vector3.up;
        if (characterController.isGrounded)
        {
            _verticalDirection.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
            return;
        }
        _verticalDirection.y += Physics.gravity.y * gravityModifier * Time.deltaTime;
    }

    private void TurnHeadTowards(Vector2 mouseInput)
    {
        transform.rotation = Quaternion.Euler(0, mouseInput.x, 0);
        headTransform.localRotation = Quaternion.Euler(mouseInput.y, 0, 0);
    }

    public void LockMovement(bool isOn)
    {
        _lockMovement = isOn;
    }
}
