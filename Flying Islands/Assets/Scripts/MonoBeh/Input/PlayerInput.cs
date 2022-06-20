using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;
    public bool combatState;

    public bool switchControll;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    private Controls controls;

    private InputAction moveAction;
    private InputAction lookAction;

    [SerializeField]
    private InputManager inputManager;

    private void Awake()
    {
        controls = inputManager.Controls;
        moveAction = controls.Player.Move;
        lookAction = controls.Player.Look;
    }

    private void OnEnable()
    {
        controls.Player.Enable();

        controls.Player.Fire.performed += Fire;
        controls.Player.Jump.performed += Jump;
        controls.Player.SwitchControll.started += SwitchControll;
    }

    private void OnDisable()
    {
        controls.Player.Fire.performed -= Fire;
        controls.Player.Jump.performed -= Jump;
        controls.Player.SwitchControll.started -= SwitchControll;

        controls.Player.Disable();
    }

    private void Update()
    {
        MoveInput(moveAction.ReadValue<Vector2>());
        LookInput(lookAction.ReadValue<Vector2>());
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        jump = true;
    }

    private void Fire(InputAction.CallbackContext ctx)
    {
        sprint = !sprint;
        combatState = !combatState;
    }

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    public void SwitchControll(InputAction.CallbackContext ctx)
    {
        switchControll = true;
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }
}