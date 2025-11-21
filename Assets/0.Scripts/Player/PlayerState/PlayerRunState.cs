using UnityEngine;

public class PlayerRunState : PlayerMovementState
{
    public PlayerRunState(PlayerController player) : base(player)
    {

    }

    public override void OnEnter()
    {
        Debug.Log("Run 상태로 전환 완료");
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        if(player.moveDir == Vector2.zero)
        {
            player.SetState(new PlayerIdleState(player));
        }
        else if (!player.isSprint)
        {
            player.SetState(new PlayerMoveState(player));
        }

    }

    public override void OnFixedUpdate()
    {
        player.Move();
    }
}
