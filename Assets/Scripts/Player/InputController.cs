using System;
using UnityEngine;

public class InputController : GameSingleton<InputController>
{
    private Vector2 _inputDirection;
    private Vector2 _mouseInput;
    private static bool _inputsLocked = true;
    private static bool _elevatorMode = true;

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
    public static event Action OnInteractionButtonPress;
    public static event Action OnCollectButtonPress;
    public static event Action OnInventoryButtonPress;
    public static event Action OnBackButtonPress;
    public static event Action OnDoorInteractionPress;
    public static event Action<int> OnElevatorButtonPress;
    public static event Action OnElevatorCall;
    public static event Action OnInGameMenuOpen;

    public delegate void InputAction();
    public InputAction RunInputScheme;

    private void Update()
    {
        if (_inputsLocked)
            return;

        RunInputScheme?.Invoke();
    }

    public void LockInputs(bool lockActive)
    {
        _inputsLocked = lockActive;
    }

    public void SetInvertMouseY(bool active)
    {
        invertY = active;
    }

    public void ElevatorMode(bool isOn)
    {
        _elevatorMode = isOn;
    }

    private void RunUiInputScheme()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            OnCollectButtonPress?.Invoke();

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("XBoxB"))
            OnBackButtonPress?.Invoke();
    }

    private void RunCastleVisitInputScheme()
    {
        _inputDirection.x = Input.GetAxis("Horizontal");
        _inputDirection.y = Input.GetAxis("Vertical");
        _inputDirection = _inputDirection.magnitude > 1 ? _inputDirection.normalized : _inputDirection;
        OnMoveInput?.Invoke(_inputDirection);

        _mouseInput.x += Input.GetAxis("Mouse X") * mouseSensitivity;
        _mouseInput.y += VerticalRotationSign * Input.GetAxis("Mouse Y") * mouseSensitivity;

        _mouseInput.x += Input.GetAxis("RightStickHorizontal") * mouseSensitivity;
        _mouseInput.y -= VerticalRotationSign * Input.GetAxis("RightStickVertical") * mouseSensitivity;
        _mouseInput.y = Mathf.Clamp(_mouseInput.y, -verticalCameraClampValue, verticalCameraClampValue);

        OnTurnInput?.Invoke(_mouseInput);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("XBoxA"))
            OnInteractionButtonPress?.Invoke();

        if (Input.GetKeyDown(KeyCode.I) || Input.GetButtonDown("XBoxBack"))
            OnInventoryButtonPress?.Invoke();

        if (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("XBoxX"))
            OnDoorInteractionPress?.Invoke();

        if (Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown("XBoxY"))
            OnElevatorCall?.Invoke();

        if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("XBoxStart"))
            OnInGameMenuOpen?.Invoke();

        if (!_elevatorMode)
            return;

        RunElevatorInputMode();
    }

    private void RunElevatorInputMode()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            OnElevatorButtonPress?.Invoke(1);

        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            OnElevatorButtonPress?.Invoke(2);

        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            OnElevatorButtonPress?.Invoke(3);

        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            OnElevatorButtonPress?.Invoke(4);

        float axisInput = Input.GetAxisRaw("XBoxDpadVertical");
        if (axisInput == 0) return;

        int nextFloor = axisInput > 0 ? 4 : 1;
        OnElevatorButtonPress?.Invoke(nextFloor);
    }

    public void SetInputScheme(UIState currentState)
    {
        switch (currentState)
        {
            case UIState.Inactive:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                RunInputScheme = RunCastleVisitInputScheme;
                break;
            case UIState.ArtifactInfo:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                RunInputScheme = RunUiInputScheme;
                break;
            case UIState.InventoryPanel:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                RunInputScheme = RunUiInputScheme;
                break;
            case UIState.InGameMenu:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                RunInputScheme = RunUiInputScheme;
                break;
            default:
                break;
        }
    }
}
