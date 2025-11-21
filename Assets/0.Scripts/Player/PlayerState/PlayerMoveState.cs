using UnityEngine;

public class PlayerMoveState : PlayerMovementState
{
    public PlayerMoveState(PlayerController player) : base(player)
    {

    }

    public override void OnEnter()
    {
        Debug.Log("Move 상태로 전환 완료");
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        if (player.moveDir == Vector2.zero)
        {
            player.SetState(new PlayerIdleState(player));
        }
        else if (player.isSprint)
        {
            player.SetState(new PlayerRunState(player));
        }
    }

    public override void OnFixedUpdate()
    {
        player.Move();
    }
}
