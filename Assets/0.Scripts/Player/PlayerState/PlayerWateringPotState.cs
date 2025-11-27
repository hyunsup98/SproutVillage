using UnityEngine;

public class PlayerWateringPotState : PlayerToolState
{
    public PlayerWateringPotState(PlayerController player) : base(player)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.SetAnimTrigger("wateringPot");
        player.SetWaterAnim();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
}
