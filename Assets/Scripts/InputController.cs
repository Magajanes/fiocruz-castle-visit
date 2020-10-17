using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Vector3 movementX, movementY;
    private Vector3 _inputDirection;
    private Vector2 _mouseInput;

    public event Action<Vector3> OnMoveInput;
    public event Action<Vector2> OnMouseInput;

    [SerializeField]
    private float mouseSensitivity;

    private void Update()
    {
        movementX = Input.GetAxis("Horizontal") * transform.right;
        movementY = Input.GetAxis("Vertical") * transform.forward;
        _inputDirection = movementX + movementY;
        OnMoveInput?.Invoke(_inputDirection);

        _mouseInput.x = Input.GetAxis("Mouse X") * mouseSensitivity;
        _mouseInput.y = Input.GetAxis("Mouse Y") * mouseSensitivity;
        OnMouseInput?.Invoke(_mouseInput);
    }
}
