using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Vector2 _inputDirection;
    private Vector2 _mouseInput;
    private static bool _inputsLocked = true;

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

    public delegate void InputAction();
    public InputAction RunInputScheme;

    private void Awake()
    {
        UIManager.OnUIStateChange += SetInputScheme;
    }

    private void Start()
    {
        SetInputScheme(UIState.Inactive);
    }

    private void Update()
    {
        if (_inputsLocked)
            return;

        RunInputScheme();
    }

    private void OnDestroy()
    {
        UIManager.OnUIStateChange -= SetInputScheme;
    }

    public static void LockInputs(bool lockActive)
    {
        _inputsLocked = lockActive;
    }

    private void RunUiInputScheme()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            OnCollectButtonPress?.Invoke();

        if (Input.GetKeyDown(KeyCode.Escape))
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
        _mouseInput.y = Mathf.Clamp(_mouseInput.y, -verticalCameraClampValue, verticalCameraClampValue);
        OnTurnInput?.Invoke(_mouseInput);

        if (Input.GetButtonDown("Jump"))
            OnInteractionButtonPress?.Invoke();

        if (Input.GetKeyDown(KeyCode.I))
            OnInventoryButtonPress?.Invoke();

        if (Input.GetKeyDown(KeyCode.F))
            OnDoorInteractionPress?.Invoke();
    }

    private void SetInputScheme(UIState currentState)
    {
        switch (currentState)
        {
            case UIState.Inactive:
                Cursor.lockState = CursorLockMode.Locked;
                RunInputScheme = RunCastleVisitInputScheme;
                break;
            case UIState.ArtifactInfo:
                Cursor.lockState = CursorLockMode.None;
                RunInputScheme = RunUiInputScheme;
                break;
            case UIState.InventoryPanel:
                Cursor.lockState = CursorLockMode.None;
                RunInputScheme = RunUiInputScheme;
                break;
            default:
                break;
        }
    }
}
