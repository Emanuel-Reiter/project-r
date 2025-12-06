using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    [Header("Transition params")]
    [SerializeField] private PlayerIdleState _idleState;

    public override void CheckExitState(PlayerStateManager player)
    {
        if (!FinishedExitTime()) return;

        if (player.Deps.Locomotion.HorizontalVel == 0f)
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
        player.Deps.Locomotion.CalculateHorizontalVel();
        player.Deps.Locomotion.CalculateVerticalVel();

        if (player.Deps.Input.IsUseItemHold) player.Deps.UseItem.Use();

        if (player.Deps.Input.IsJumpPressed) player.Deps.Locomotion.Jump();
    }

    public override void FixedUpdateState(PlayerStateManager player)
    {
        player.Deps.Locomotion.Move();
        player.Deps.Locomotion.RotateTowardsMovement();
    }
}
