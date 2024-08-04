using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    private static InputController _instance;
    public static InputController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InputController>();
            }

            return _instance;
        }
    }

    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Collider2D _playerCollider;

    [HideInInspector] public UnityEvent StartGameEvent;

    public Vector2 InputMove;

    private float MouseToKeyboardModifier = 0.07f;
    private float DeadZone = 2;
    private Vector2 inputMousePosition;
    private bool isTouchInZone;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    private void Start()
    {
        _playerInput.actions["Move"].performed += OnMove;
        _playerInput.actions["Move"].canceled += OnStop;

        _playerInput.actions["MousePosition"].performed += MousePosition;
        _playerInput.actions["MouseMove"].performed += MouseMove;
        _playerInput.actions["MousePress"].performed += MousePress;
        _playerInput.actions["MousePress"].canceled += MousePressStop;

        _playerInput.actions["Start"].performed += StartGame;
    }

    private void StartGame(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.IsGameNeedStart)
        StartGameEvent.Invoke();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        InputMove = context.ReadValue<Vector2>();
    }

    private void OnStop(InputAction.CallbackContext context)
    {
        InputMove = Vector2.zero;
    }

    private void MousePosition(InputAction.CallbackContext context)
    {
        if (!isTouchInZone)
        {
            inputMousePosition = context.ReadValue<Vector2>();
        }
    }

    private void MouseMove(InputAction.CallbackContext context)
    {
        if (isTouchInZone)
        {
            if (Mathf.Abs(context.ReadValue<Vector2>().x) < DeadZone)
            {
                InputMove = Vector2.zero;
            }
            else  InputMove = context.ReadValue<Vector2>() * MouseToKeyboardModifier;
        }
    }

    private void MousePress(InputAction.CallbackContext context)
    {
        isTouchInZone = _playerCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(inputMousePosition));
    }

    private void MousePressStop(InputAction.CallbackContext context)
    {
        isTouchInZone = false;
        InputMove = Vector2.zero;
    }
}
