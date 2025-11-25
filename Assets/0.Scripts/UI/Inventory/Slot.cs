using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType
{
    InvenSlot,          //인벤 슬롯
    QuickSlot,          //게임 화면 하단에 보이는 퀵슬롯
    QuickSlot_Inven,    //인벤 UI 내에 존재하는 퀵슬롯
}

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private SlotType slotType;
    [SerializeField] private RectTransform slotTrans;       //현재 슬롯의 RectTransform

    private void Awake()
    {
        if (slotTrans == null)
            TryGetComponent(out slotTrans);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {

    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
