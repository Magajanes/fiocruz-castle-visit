using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Vector2 _inputDirection;
    private Vector2 _mouseInput;

    [Header("Player Movement")]
    [SerializeField]
    private float mouseSensitivity;
    [SerializeField]
    private float verticalCameraClampValue;
    [SerializeField]
    private bool invertY = true;

    private short VerticalRotationSign => invertY ? (short)1 : (short)-1;

    public static event Action<Vector2> OnMoveInput;
    public static event Action<Vector2> OnTurnInput;

    private void Update()
    {
        _inputDirection.x = Input.GetAxis("Horizontal");
        _inputDirection.y = Input.GetAxis("Vertical");
        _inputDirection = _inputDirection.magnitude > 1 ? _inputDirection.normalized : _inputDirection;
        OnMoveInput?.Invoke(_inputDirection);

        _mouseInput.x += Input.GetAxis("Mouse X") * mouseSensitivity;
        _mouseInput.y += VerticalRotationSign * Input.GetAxis("Mouse Y") * mouseSensitivity;
        _mouseInput.y = Mathf.Clamp(_mouseInput.y, -verticalCameraClampValue, verticalCameraClampValue);
        OnTurnInput?.Invoke(_mouseInput);
    }
}
