using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    public Slot refSlot { get; private set; }   //현재 참조중인 슬롯
    [SerializeField] private Image itemImg;

    public void SetDragSlot(Slot slot)
    {
        if (slot.SlotItem == null)
        {
            refSlot = null;
            SetColor(0);
            return;
        }

        refSlot = slot;
        itemImg.sprite = refSlot.SlotItem.itemData.ItemSprite;
        SetColor(1);
        transform.SetAsLastSibling();
    }

    public void ClearDragSlot()
    {
        refSlot = null;
        SetColor(0);
    }

    private void SetColor(float alpha)
    {
        Color color = itemImg.color;
        color.a = alpha;
        itemImg.color = color;
    }
}
