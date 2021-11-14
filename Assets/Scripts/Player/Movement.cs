using UnityEngine;

public class Movement : MonoBehaviour
{
    public const string CHARACTER_SOUNDS_BUNDLE_PATH = "SoundBundles/CharacterSounds";
    public const string STEP_SOUND_ID = "step";
    
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
    private bool _isMoving;

    private SoundsManager.ChannelType _currentChannel;
    private SoundsBundle _soundsBundle;
    private AudioClip _stepSound;

    private void Awake()
    {
        Instance = this;

        InputController.OnMoveInput -= MoveTowards;
        InputController.OnMoveInput += MoveTowards;

        InputController.OnTurnInput -= TurnHeadTowards;
        InputController.OnTurnInput += TurnHeadTowards;
    }

    private void Start()
    {
        SoundsManager.LoadSoundsBundle(CHARACTER_SOUNDS_BUNDLE_PATH, OnSoundsLoaded);

        void OnSoundsLoaded(SoundsBundle bundle)
        {
            _soundsBundle = bundle;
            _stepSound = _soundsBundle.GetAudioClipById(STEP_SOUND_ID);
        }
    }

    private void OnDestroy()
    {
        InputController.OnMoveInput -= MoveTowards;
        InputController.OnTurnInput -= TurnHeadTowards;

        //Clear sound assets
        _stepSound = null;
        Resources.UnloadAsset(_soundsBundle);
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

        if (!_isMoving && characterController.velocity.magnitude > 0)
        {
            _isMoving = true;
            SoundsManager.Instance.PlaySFXLoop(_stepSound, out _currentChannel);
            return;
        }

        if (_isMoving && characterController.velocity.magnitude < 0.1f)
        {
            _isMoving = false;
            SoundsManager.Instance.StopSFXLoop(_currentChannel);
        }
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
