using UnityEngine;

/// <summary>
/// 플레이어가 도구를 장착하고 있고 해당 도구로 특정 행동을 하는 상태 ex) 도끼 휘두르기, 괭이로 괭이질하기
/// </summary>
public class PlayerToolState : IState
{
    protected PlayerController player;

    public PlayerToolState(PlayerController player)
    {
        this.player = player;
    }

    public virtual void OnEnter()
    {
        
    }

    public virtual void OnExit()
    {
        
    }

    public virtual void OnUpdate()
    {
        
    }

    public virtual void OnFixedUpdate()
    {
        
    }
}
