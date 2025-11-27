using UnityEngine;

public class PlayerDashState : PlayerMovementState
{
    public PlayerDashState(PlayerController player) : base(player)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.Dash();
    }


    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if(!player.isDash)
        {
            player.SetState(new PlayerIdleState(player));
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
}
