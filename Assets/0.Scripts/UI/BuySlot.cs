using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuySlot : MonoBehaviour
{
    [SerializeField] private Inventory inventory;

    [SerializeField] private Item item;

    [SerializeField] private Image itemImg;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text priceText;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        itemImg.sprite = item.itemData.ItemSprite;
        nameText.text = item.itemData.ItemName;
        priceText.text = item.itemData.buyPrice.ToString();
    }

    public void OnClickBuy()
    {
        if(GameManager.Instance.Player.Gold >= item.itemData.buyPrice)
        {
            GameManager.Instance.Player.Gold -= item.itemData.buyPrice;
            var buyItem = ItemPool.Instance.GetObjects(item, ItemPool.Instance.transform);
            inventory.AddItem(buyItem);
        }
    }
}
