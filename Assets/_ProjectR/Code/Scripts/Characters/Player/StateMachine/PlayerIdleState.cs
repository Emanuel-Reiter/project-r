using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    [Header("Transition params")]
    [SerializeField] private PlayerMoveState _moveState;
    [SerializeField] private PlayerFallState _fallState;
    [SerializeField] private PlayerJumpState _jumpState;

    public override void CheckExitState(PlayerStateManager player)
    {
        if (!FinishedExitTime()) return;

        if (!player.Deps.Locomotion.IsGrounded)
        {
            player.SwitchState(_fallState);
            return;
        }

        if (player.Deps.Input.IsJumpPressed && player.Deps.Locomotion.CanJump()) 
        {
            player.SwitchState(_jumpState);
            return;
        }

        if (player.Deps.Locomotion.HorizontalVel != 0f)
        {
            player.SwitchState(_moveState);
            return;
        }
    }

    public override void EnterState(PlayerStateManager player)
    {
        player.Deps.Animation.PlayBaseAnimation(player.Deps.Animation.IdleBaseAnim);
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

    }
}
