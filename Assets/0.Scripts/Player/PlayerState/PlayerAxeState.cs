using UnityEngine;

public class PlayerAxeState : PlayerToolState
{
    public PlayerAxeState(PlayerController player) : base(player)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.SetAnimTrigger("axe");
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
