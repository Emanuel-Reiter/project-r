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
        _playerInputActions.Default.Jump.started += ProcessPerformedJumpInput;
        _playerInputActions.Default.Jump.canceled += ProcessCanceledJumpInput;

        // Sprint (Hold)
        _playerInputActions.Default.Sprint.performed += ProcessPerformedSprintInput;
        _playerInputActions.Default.Sprint.canceled += ProcessCanceledSprintInput;

        // Dash
        _playerInputActions.Default.Dash.started += ProcessPerformedDashInput;
        _playerInputActions.Default.Dash.canceled += ProcessCanceledDashInput;

        // Attack Light
        _playerInputActions.Default.UseItem.started += ProcessPerformedAttackLightInput;
        _playerInputActions.Default.UseItem.canceled += ProcessCanceledAttackLightInput;

        // Interact
        _playerInputActions.Default.Interact.started += ProcessPerformedInteractInput;
        _playerInputActions.Default.Interact.canceled += ProcessCanceledInteractInput;

        // Inventory
        _playerInputActions.Default.Inventory.started += ProcessPerformedInventoryInput;
        _playerInputActions.Default.Inventory.canceled += ProcessCanceledInventoryInput;
    }

    private void UnsubscribeFromAllActions()
    {
        // Jump
        _playerInputActions.Default.Jump.started -= ProcessPerformedJumpInput;
        _playerInputActions.Default.Jump.canceled -= ProcessCanceledJumpInput;

        // Sprint
        _playerInputActions.Default.Sprint.performed -= ProcessPerformedSprintInput;
        _playerInputActions.Default.Sprint.canceled -= ProcessCanceledSprintInput;

        // Dash
        _playerInputActions.Default.Dash.started -= ProcessPerformedDashInput;
        _playerInputActions.Default.Dash.canceled -= ProcessCanceledDashInput;

        // Attack Light
        _playerInputActions.Default.UseItem.started -= ProcessPerformedAttackLightInput;
        _playerInputActions.Default.UseItem.canceled -= ProcessCanceledAttackLightInput;

        // Interact
        _playerInputActions.Default.Interact.started -= ProcessPerformedInteractInput;
        _playerInputActions.Default.Interact.canceled -= ProcessCanceledInteractInput;

        // Inventory
        _playerInputActions.Default.Inventory.started -= ProcessPerformedInventoryInput;
        _playerInputActions.Default.Inventory.canceled -= ProcessCanceledInventoryInput;
    }

    // Jump
    private void ProcessPerformedJumpInput(InputAction.CallbackContext context) { StartCoroutine(ProcessPerformedJumpInputCoroutine()); }
    private void ProcessCanceledJumpInput(InputAction.CallbackContext context) { _isJumpPressed = false; }

    // Sprint 
    private void ProcessPerformedSprintInput(InputAction.CallbackContext context) { _isSprintHold = true; }
    private void ProcessCanceledSprintInput(InputAction.CallbackContext context) { _isSprintHold = false; }

    // Dash
    private void ProcessPerformedDashInput(InputAction.CallbackContext context) { StartCoroutine(ProcessPerformedDashInputCoroutine()); }
    private void ProcessCanceledDashInput(InputAction.CallbackContext context) { _isDashPressed = false; }

    // Attack Light
    private void ProcessPerformedAttackLightInput(InputAction.CallbackContext context) { StartCoroutine(ProcessPerformedAttackLightInputtCoroutine()); }
    private void ProcessCanceledAttackLightInput(InputAction.CallbackContext context) { _isUseItemPressed = false; }

    // Interact
    private void ProcessPerformedInteractInput(InputAction.CallbackContext context) { StartCoroutine(ProcessPerformedInteractInputCoroutine()); }
    private void ProcessCanceledInteractInput(InputAction.CallbackContext context) { _isInteractPressed = false; }

    // Inventory
    private void ProcessPerformedInventoryInput(InputAction.CallbackContext context) { StartCoroutine(ProcessPerformedInventoryInputCoroutine()); }
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
    private IEnumerator ProcessPerformedJumpInputCoroutine()
    {
        _isJumpPressed = true;
        yield return null;
        // Check if object still exists
        if (this != null)
        {
            _isJumpPressed = false;
        }
    }

    // Dash
    private IEnumerator ProcessPerformedDashInputCoroutine()
    {
        _isDashPressed = true;
        yield return null;
        if (this != null) _isDashPressed = false;
    }

    // Attack Light
    private IEnumerator ProcessPerformedAttackLightInputtCoroutine()
    {
        _isUseItemPressed = true;
        yield return null;
        if (this != null) _isUseItemPressed = false;
    }

    // Interact
    private IEnumerator ProcessPerformedInteractInputCoroutine()
    {
        _isInteractPressed = true;
        yield return null;
        if (this != null) _isInteractPressed = false;
    }

    // Inventory
    private IEnumerator ProcessPerformedInventoryInputCoroutine()
    {
        _isInventoryPressed = true;
        yield return null;
        if (this != null) _isInventoryPressed = false;
    }
}
