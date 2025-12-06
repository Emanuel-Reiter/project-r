using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    [SerializeField] private PlayerIdleState _idleState;
    [SerializeField] private PlayerMoveState _moveState;
    [SerializeField] private PlayerJumpState _jumpState;

    public override void CheckExitState(PlayerStateManager player)
    {
        if (!FinishedExitTime()) return;

        if(player.Deps.Input.IsJumpPressed && player.Deps.Locomotion.CanJump())
        {
            player.SwitchState(_jumpState);
            return;
        }

        if (!player.Deps.Locomotion.IsGrounded) return;

        if (player.Deps.Locomotion.HorizontalVel != 0f)
        {
            player.SwitchState(_moveState);
            return;
        }
        else
        {
            player.SwitchState(_idleState);
            return;
        }
    }

    public override void EnterState(PlayerStateManager player)
    {
        InitializeState();

        player.Deps.Animation.PlayBaseAnimation(player.Deps.Animation.FallBaseAnim);
    }

    public override void ExitState(PlayerStateManager player)
    {

    }

    public override void UpdateState(PlayerStateManager player)
    {
        if (player.Deps.Input.IsUseItemHeld) player.Deps.UseItem.Use();
    }

    public override void FixedUpdateState(PlayerStateManager player)
    {
        player.Deps.Locomotion.RotateTowardsMovement();
    }
}
