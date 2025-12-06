using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    [Header("Transition params")]
    [SerializeField] private PlayerFallState _fallState;
    
    public override void CheckExitState(PlayerStateManager player)
    {
        if (!FinishedExitTime()) return;
        
        if (player.Deps.Locomotion.VerticalVel < 0f)
        {
            player.SwitchState(_fallState);
            return;
        }

        if (player.Deps.Input.IsJumpPressed && player.Deps.Locomotion.CanJump())
        {
            player.SwitchState(this);
            return;
        }
    }

    public override void EnterState(PlayerStateManager player)
    {
        player.Deps.Locomotion.Jump();
        player.Deps.Animation.PlayBaseAnimation(player.Deps.Animation.JumpBaseAnim);
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
