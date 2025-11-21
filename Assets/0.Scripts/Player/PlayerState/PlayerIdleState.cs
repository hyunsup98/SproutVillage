using UnityEngine;

public class PlayerIdleState : PlayerMovementState
{
    public PlayerIdleState(PlayerController player) : base(player)
    {

    }

    public override void OnEnter()
    {
        Debug.Log("Idle 상태로 전환 완료");
    }


    public override void OnExit() { }

    public override void OnUpdate()
    {
        if(player.moveDir != Vector2.zero)
        {
            //플레이어가 이동 중일 때
            if (player.isSprint)
            {
                //플레이어가 왼쪽 쉬프트를 눌렀다면 → 달리는 중이라면
                player.SetState(new PlayerRunState(player));
            }
            else
            {
                //플레이어가 왼쪽 쉬프트를 누르지 않았다면 → 걷는 중이라면
                player.SetState(new PlayerMoveState(player));
            }
        }
    }

    public override void OnFixedUpdate()
    {
        player.Move();
    }

}
