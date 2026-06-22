using UnityEngine;

public class WateringPot : Tool
{
    public override void SetState(PlayerController player)
    {
        playerToolState = new PlayerWateringPotState(player);
    }

    public override void Activate()
    {
        Debug.Log("π∞ ¡÷±‚!");
        TileManager.Instance.SetTileToWetSoil();
    }
}
