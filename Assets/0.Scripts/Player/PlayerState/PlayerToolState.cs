using UnityEngine;

/// <summary>
/// 플레이어가 도구를 장착하고 있고 해당 도구로 특정 행동을 하는 상태 ex) 도끼 휘두르기, 괭이로 괭이질하기
/// PlayerHoeState, PlayerAxeState, PlayerWteringPotState
/// </summary>
public class PlayerToolState : IState
{
    protected PlayerController player;
    protected float coolTimer = 0;      //도구의 쿨타임이 지나면 상태를 해제하기 위한 타이머

    public PlayerToolState(PlayerController player)
    {
        this.player = player;
    }

    public virtual void OnEnter()
    {
        GameManager.Instance.CurrentGameState = GameState.InteractionLock;
        player.SetAnimBlend(("idleX", player.idleDir.x), ("idleY", player.idleDir.y));
    }

    public virtual void OnExit()
    {
        coolTimer = 0;      //도구 상태를 캐싱했기 때문에 다음에 다시 사용하기 위해 0으로 초기화
        GameManager.Instance.CurrentGameState = GameState.Playing;
    }

    public virtual void OnUpdate()
    {
        if (coolTimer > player.CurrentTool.coolTime)
            player.SetState(new PlayerIdleState(player));

        coolTimer += Time.deltaTime;
    }

    public virtual void OnFixedUpdate()
    {
        
    }
}
