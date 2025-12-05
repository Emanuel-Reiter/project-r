using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    [Header("Transition params")]
    [SerializeField] private PlayerIdleState _idleState;

    public override void CheckExitState(PlayerStateManager player)
    {
        if (!FinishedExitTime()) return;

        if (player.Deps.Locomotion.VelocityMagnitude <= 0f)
        {
            player.SwitchState(_idleState);
            return;
        }
    }

    public override void EnterState(PlayerStateManager player)
    {
        InitializeState();

        player.Deps.Animation.PlayBaseAnimation(player.Deps.Animation.MoveLegsAnim);
    }

    public override void ExitState(PlayerStateManager player)
    {

    }

    public override void UpdateState(PlayerStateManager player)
    {
        if (player.Deps.Input.IsUseItemPressed) player.Deps.UseItem.Use();
    }

    public override void FixedUpdateState(PlayerStateManager player)
    {
        player.Deps.Locomotion.Move();
        player.Deps.Locomotion.RotateTowardsMovement();
    }
}
