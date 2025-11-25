using UnityEngine;

public class Hoe : Tool
{
    public override void SetState(PlayerController player)
    {
        playerToolState = new PlayerHoeState(player);
    }

    public override void Activate()
    {
        Debug.Log("±ªÀÌ ÈÖµÎ¸£±â!");
        TileManager.Instance.GetTileToSoil();
    }
}
