using UnityEngine;

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

    }

    public virtual void OnFixedUpdate() { }
}
