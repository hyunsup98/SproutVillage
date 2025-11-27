using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/ItemSO")]
public class ItemSO : ScriptableObject
{
    [field: SerializeField] public string ItemName { get; private set; }
    [field: SerializeField] public int ItemId { get; private set; }
    [field: SerializeField] public Sprite ItemSprite { get; private set; }
    [field: SerializeField] public string ItemInfo { get; private set; }
    [field: SerializeField] public bool IsStackable { get; private set; }
    [field: SerializeField] public int maxCount { get; private set; } = 999;
    [field: SerializeField] public int buyPrice { get; private set; }
    [field: SerializeField] public int sellPrice { get; private set; }
}
