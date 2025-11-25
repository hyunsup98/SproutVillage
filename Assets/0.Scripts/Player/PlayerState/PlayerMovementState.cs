using UnityEngine;

/// <summary>
/// 이동과 관련된 상태들을 파생하고 있는 클래스
/// PlayerMoveState, PlayerRunState
/// </summary>
public abstract class PlayerMovementState : IState
{
    protected PlayerController player;

    public PlayerMovementState(PlayerController player)
    {
        this.player = player;
    }

    public virtual void OnEnter()
    {
        //moveDir의 값이 true면 move 애니메이션 재생
        player.SetAnimBool("isMove", player.moveDir != Vector2.zero);
    }

    public virtual void OnExit()
    {
        player.Move();
    }

    public virtual void OnUpdate()
    {
        //현재 들고있는 도구 사용 상태로 변경
        if (player.isToolInteracted)
        {
            player.SetState(player.CurrentTool.playerToolState);
        }
    }

    public virtual void OnFixedUpdate() { }
}
