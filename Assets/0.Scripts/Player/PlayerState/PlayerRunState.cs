using UnityEngine;

public class PlayerRunState : PlayerMovementState
{
    public PlayerRunState(PlayerController player) : base(player)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if(player.moveDir == Vector2.zero)
        {
            player.SetState(new PlayerIdleState(player));
        }
        else
        {
            if(player.isDash)
            {
                player.SetState(new PlayerDashState(player));
            }

            if (!player.isSprint)
            {
                player.SetState(new PlayerMoveState(player));
            }

            //현재 들고있는 도구 사용 상태로 변경
            if (player.isToolInteracted)
            {
                player.SetState(player.CurrentTool.playerToolState);
            }
        }
    }

    public override void OnFixedUpdate()
    {
        player.Move();
    }
}
