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
    public static event Action OnInteractionButtonPress;
    public static event Action OnCloseButtonPress;

    public delegate void InputAction();
    public InputAction RunInputScheme;

    private void Awake()
    {
        UIManager.OnUIStateChange += SetInputScheme;
    }

    private void Start()
    {
        RunInputScheme = RunCastleVisitInputScheme;
    }

    private void Update()
    {
        RunInputScheme();
    }

    private void OnDestroy()
    {
        UIManager.OnUIStateChange -= SetInputScheme;
    }

    private void RunArtifactInfoInputScheme()
    {
        if (Input.GetButtonDown("Fire1"))
            OnCloseButtonPress?.Invoke();
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
    }

    private void SetInputScheme(UIState currentState)
    {
        switch (currentState)
        {
            case UIState.Inactive:
                RunInputScheme = RunCastleVisitInputScheme;
                break;
            case UIState.ArtifactInfo:
                RunInputScheme = RunArtifactInfoInputScheme;
                break;
            default:
                break;
        }
    }
}
