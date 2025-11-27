using UnityEngine;

public class Egg : Item, IUsable
{
    [SerializeField] private AnimalAI animal;

    public void Use(Vector3 pos)
    {
        Vector3Int tilePos = Vector3Int.FloorToInt(pos);

        if (TileManager.Instance.HasTile(TileManager.Instance.groundTilemap, tilePos))
        {
            var _animal = Instantiate(animal);
            _animal.transform.position = new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0f);

            Count--;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            UIManager.Instance.InvenScripts.AddItem(this);
            transform.position = new Vector3(100f, 100f, 0f);
        }
    }
}
