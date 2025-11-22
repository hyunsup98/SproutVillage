using UnityEngine;

public class Axe : Tool
{
    public override void SetState(PlayerController player)
    {
        playerToolState = new PlayerHoeState(player);
    }

    public override void Activate()
    {

    }
}
