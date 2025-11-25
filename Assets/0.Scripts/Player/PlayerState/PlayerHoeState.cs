using UnityEngine;

//±ªÀÌ¸¦ ÈÖµÎ¸£´Â »óÅÂ
public class PlayerHoeState : PlayerToolState
{
    public PlayerHoeState(PlayerController player) : base(player)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.SetAnimTrigger("hoe");
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
