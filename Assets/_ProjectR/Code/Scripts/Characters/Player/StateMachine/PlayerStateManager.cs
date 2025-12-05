using Unity.Netcode;
using UnityEngine;

public class PlayerStateManager : NetworkBehaviour
{
    // State management
    [SerializeField] private PlayerBaseState _initialState;
    public PlayerBaseState CurrentState { get; private set; }
    public PlayerBaseState PreviousState { get; private set; }

    // Dependencies
    private PlayerDependencies _dependencies;
    public PlayerDependencies Deps => _dependencies;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        _dependencies = GetComponent<PlayerDependencies>();

        // Set the initial state
        SwitchState(_initialState);
    }

    private void Update()
    {
        if (!IsOwner) return;

        CurrentState?.CheckExitState(this);
        CurrentState?.UpdateState(this);
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        CurrentState?.FixedUpdateState(this);
    }

    public void SwitchState(PlayerBaseState newState)
    {
        if (!IsOwner) return;

        if (newState == null) return;

        CurrentState?.ExitState(this);
        PreviousState = CurrentState;

        CurrentState = newState;
        CurrentState.InitializeState();
        CurrentState?.EnterState(this);
    }

    public bool WasPreviousState<T>() where T : PlayerBaseState { return PreviousState is T; }

    public bool IsCurrentState<T>() where T : PlayerBaseState { return CurrentState is T; }
}