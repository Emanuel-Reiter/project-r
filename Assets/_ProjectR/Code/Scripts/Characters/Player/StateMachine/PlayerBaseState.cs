using Unity.Netcode;
using UnityEngine;

public abstract class PlayerBaseState : NetworkBehaviour
{
    public bool IsStateComplete { get; protected set; } = false;

    [Header("State prarms")]
    public bool CanUseItem = false;

    [Header("Exit params")]
    [SerializeField] private bool _useExitTime = false;
    [SerializeField] private float _stateDuration = 1.0f;

    private float _startStateTime;
    private float _currentStateTime => Time.time - _startStateTime;

    public void InitializeState() { _startStateTime = Time.time; IsStateComplete = false; }
    public void SetStateDuration(float stateDuration) { _stateDuration = stateDuration; }
    public bool FinishedExitTime()
    {
        if (!_useExitTime) return true;
        else return _currentStateTime >= _stateDuration;
    }
    
    public abstract void CheckExitState(PlayerStateManager player);
    public abstract void EnterState(PlayerStateManager player);
    public abstract void ExitState(PlayerStateManager player);
    public abstract void UpdateState(PlayerStateManager player);
    public abstract void FixedUpdateState(PlayerStateManager player);
}
