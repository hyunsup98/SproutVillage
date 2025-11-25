using UnityEngine;

public class Axe : Tool
{
    public override void SetState(PlayerController player)
    {
        playerToolState = new PlayerAxeState(player);
    }

    public override void Activate()
    {
        Debug.Log("µµ≥¢ »÷µŒ∏£±‚!");
    }
}
