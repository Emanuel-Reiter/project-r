using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;

    // Movement
    private Vector2 _movementDirection = Vector2.zero;
    public Vector2 MovementDirection => _movementDirection;

    private bool _isJumpPressed = false;
    public bool IsJumpPressed => _isJumpPressed;
    
    private bool _isJumpHeld = false;
    public bool IsJumpHeld => _isJumpHeld;

    private bool _isSprintHold = false;
    public bool SprintHold => _isSprintHold;

    private bool _isDashPressed = false;
    public bool IsDashPressed => _isDashPressed;

    // Camera
    private Vector2 _cameraLook = Vector2.zero;
    public Vector2 CameraLook => _cameraLook;

    // Attack
    private bool _isUseItemPressed = false;
    public bool IsUseItemPressed => _isUseItemPressed;

    private bool _isUseItemHeld = false;
    public bool IsUseItemHeld => _isUseItemHeld;

    // Other
    private bool _isInteractPressed = false;
    public bool InteractPressed => _isInteractPressed;
    
    private bool _isInventoryPressed = false;
    public bool InventoryPressed => _isInventoryPressed;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Default.Enable();
        SubscribeToAllActions();
    }

    private void OnDestroy()
    {
        UnsubscribeFromAllActions();
        _playerInputActions?.Default.Disable();
        _playerInputActions?.Dispose();
    }

    private void Update()
    {
        ProcessMovementDirectionInput();
        ProcessCameraDirectionInput();
    }

    private void SubscribeToAllActions()
    {
        // Jump
        _playerInputActions.Default.Jump.started += ProcessStartedJumpInput;
        _playerInputActions.Default.Jump.canceled += ProcessCanceledJumpInput;

        // Sprint (Hold)
        _playerInputActions.Default.Sprint.performed += ProcessPerformedSprintInput;
        _playerInputActions.Default.Sprint.canceled += ProcessCanceledSprintInput;

        // Dash
        _playerInputActions.Default.Dash.started += ProcessStartedDashInput;
        _playerInputActions.Default.Dash.canceled += ProcessCanceledDashInput;

        // Use Item
        _playerInputActions.Default.UseItem.started += ProcessStartedUseItemInput;
        _playerInputActions.Default.UseItem.canceled += ProcessCanceledUseItemInput;

        // Interact
        _playerInputActions.Default.Interact.started += ProcessStartedInteractInput;
        _playerInputActions.Default.Interact.canceled += ProcessCanceledInteractInput;

        // Inventory
        _playerInputActions.Default.Inventory.started += ProcessStartedInventoryInput;
        _playerInputActions.Default.Inventory.canceled += ProcessCanceledInventoryInput;
    }

    private void UnsubscribeFromAllActions()
    {
        // Jump
        _playerInputActions.Default.Jump.started -= ProcessStartedJumpInput;
        _playerInputActions.Default.Jump.canceled -= ProcessCanceledJumpInput;

        // Sprint
        _playerInputActions.Default.Sprint.performed -= ProcessPerformedSprintInput;
        _playerInputActions.Default.Sprint.canceled -= ProcessCanceledSprintInput;

        // Dash
        _playerInputActions.Default.Dash.started -= ProcessStartedDashInput;
        _playerInputActions.Default.Dash.canceled -= ProcessCanceledDashInput;

        // Use Item
        _playerInputActions.Default.UseItem.started -= ProcessStartedUseItemInput;
        _playerInputActions.Default.UseItem.canceled -= ProcessCanceledUseItemInput;

        // Interact
        _playerInputActions.Default.Interact.started -= ProcessStartedInteractInput;
        _playerInputActions.Default.Interact.canceled -= ProcessCanceledInteractInput;

        // Inventory
        _playerInputActions.Default.Inventory.started -= ProcessStartedInventoryInput;
        _playerInputActions.Default.Inventory.canceled -= ProcessCanceledInventoryInput;
    }

    // Jump
    private void ProcessStartedJumpInput(InputAction.CallbackContext context)
    {
        StartCoroutine(ProcessStartedJumpInputCoroutine());
        _isJumpHeld = true;
    }
    private void ProcessCanceledJumpInput(InputAction.CallbackContext context)
    {
        _isJumpPressed = false;
        _isJumpHeld = false;
    }

    // Sprint 
    private void ProcessPerformedSprintInput(InputAction.CallbackContext context) { _isSprintHold = true; }
    private void ProcessCanceledSprintInput(InputAction.CallbackContext context) { _isSprintHold = false; }

    // Dash
    private void ProcessStartedDashInput(InputAction.CallbackContext context) { StartCoroutine(ProcessStartedDashInputCoroutine()); }
    private void ProcessCanceledDashInput(InputAction.CallbackContext context) { _isDashPressed = false; }

    // Use Item
    private void ProcessStartedUseItemInput(InputAction.CallbackContext context) { 
        StartCoroutine(ProcessStartedUseItemInputCoroutine());
        _isUseItemHeld = true;
    }
    private void ProcessCanceledUseItemInput(InputAction.CallbackContext context) 
    { 
        _isUseItemPressed = false;
        _isUseItemHeld = false; 
    }

    // Interact
    private void ProcessStartedInteractInput(InputAction.CallbackContext context) { StartCoroutine(ProcessStartedInteractInputCoroutine()); }
    private void ProcessCanceledInteractInput(InputAction.CallbackContext context) { _isInteractPressed = false; }

    // Inventory
    private void ProcessStartedInventoryInput(InputAction.CallbackContext context) { StartCoroutine(ProcessStartedInventoryInputCoroutine()); }
    private void ProcessCanceledInventoryInput(InputAction.CallbackContext context) { _isInventoryPressed = false; }

    // Movement Direction
    private void ProcessMovementDirectionInput()
    {
        _movementDirection = _playerInputActions.Default.Movement.ReadValue<Vector2>();
    }

    // Camera Look
    private void ProcessCameraDirectionInput()
    {
        _cameraLook = _playerInputActions.Default.CameraLook.ReadValue<Vector2>();
    }

    // Jump
    private IEnumerator ProcessStartedJumpInputCoroutine()
    {
        _isJumpPressed = true;
        yield return null;
        if (this != null)
        {
            _isJumpPressed = false;
        }
    }

    // Dash
    private IEnumerator ProcessStartedDashInputCoroutine()
    {
        _isDashPressed = true;
        yield return null;
        if (this != null) _isDashPressed = false;
    }

    // Use Item
    private IEnumerator ProcessStartedUseItemInputCoroutine()
    {
        _isUseItemPressed = true;
        yield return null;
        if (this != null) _isUseItemPressed = false;
    }

    // Interact
    private IEnumerator ProcessStartedInteractInputCoroutine()
    {
        _isInteractPressed = true;
        yield return null;
        if (this != null) _isInteractPressed = false;
    }

    // Inventory
    private IEnumerator ProcessStartedInventoryInputCoroutine()
    {
        _isInventoryPressed = true;
        yield return null;
        if (this != null) _isInventoryPressed = false;
    }
}
