using UnityEngine;

public class Seed : Item, IUsable
{
    //심었을 때 생성할 작물
    [SerializeField] private Plant plant;

    //씨앗 심기
    public void Use(Vector3 pos)
    {
        Vector3Int tilePos = Vector3Int.FloorToInt(pos);

        if (TileManager.Instance.HasTile(TileManager.Instance.soilTilemap, tilePos, TileManager.Instance.groundTile))
        {
            var tilePlant = PlantPool.Instance.GetObjects(plant, PlantPool.Instance.transform);
            tilePlant.Init(tilePos);
            TileManager.Instance.SetTile(TileManager.Instance.plantTilemap, tilePos, tilePlant.gameObject);

            Count--;
        }
    }
}
