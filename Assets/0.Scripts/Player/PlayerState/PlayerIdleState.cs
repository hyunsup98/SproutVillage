using UnityEngine;

/// <summary>
/// 정지 상태일 때의 상태
/// </summary>
public class PlayerIdleState : IState
{
    private PlayerController player;

    public PlayerIdleState(PlayerController player)
    {
        this.player = player;
    }

    public void OnEnter()
    {
        //moveDir의 값이 false면 idle 애니메이션 재생
        player.SetAnimBool("isMove", player.moveDir != Vector2.zero);
        player.SetAnimBlend(("idleX", player.idleDir.x), ("idleY", player.idleDir.y));
    }


    public void OnExit() { }

    public void OnUpdate()
    {
        //현재 들고있는 도구 사용 상태로 변경
        if (player.isToolInteracted)
        {
            player.SetState(player.CurrentTool.playerToolState);
        }

        if (player.moveDir != Vector2.zero)
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

    public void OnFixedUpdate() { }
}
