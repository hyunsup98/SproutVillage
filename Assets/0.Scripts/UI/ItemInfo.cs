using TMPro;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text itemNameText;     //아이템 이름 텍스트
    [SerializeField] private TMP_Text itemInfoText;     //아이템 설명 텍스트
    [SerializeField] private TMP_Text eachPriceText;    //아이템 개당 가격 텍스트
    [SerializeField] private TMP_Text totalPriceText;   //아이템 총 가격 텍스트

    [field: SerializeField] public RectTransform itemInfoRect;
    [SerializeField] private GameObject itemInfoObj;    

    private void Awake()
    {
        if (itemInfoRect == null)
            TryGetComponent(out itemInfoRect);

        if (itemInfoObj == null)
            itemInfoObj = transform.GetChild(0).gameObject;
    }

    public void ShowItemInfo(Item item)
    {
        itemInfoObj.SetActive(true);

        itemNameText.text = item.itemData.ItemName;
        itemInfoText.text = item.itemData.ItemInfo;
        eachPriceText.text = $"개당 가격: {item.itemData.sellPrice}";
        totalPriceText.text = $"총 가격: {(item.itemData.sellPrice * item.Count)}";
    }

    public void HideItemInfo()
    {
        itemInfoObj.SetActive(false);
    }
}
