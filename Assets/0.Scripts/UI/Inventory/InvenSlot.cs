using UnityEngine;
using UnityEngine.EventSystems;

public sealed class InvenSlot : Slot, IPointerClickHandler
{
    [SerializeField] private ItemInfo itemInfo;
    [SerializeField] private SellCount sellCount;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right && SlotItem != null)
        {
            sellCount.gameObject.SetActive(true);
            sellCount.Init(this);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        //슬롯에 아이템이 있고 드래그중이 아닐 때
        if(SlotItem != null && !eventData.dragging)
        {
            float widthStandard = Screen.width - (itemInfo.itemInfoRect.sizeDelta.x + (slotTrans.sizeDelta.x / 2));
            float heightStandard = Screen.height - itemInfo.itemInfoRect.sizeDelta.y;

            float anchorX, anchorY, pivotX, pivotY;

            if(transform.position.x < widthStandard)
            {
                anchorX = 1;
                pivotX = 0;
            }
            else
            {
                anchorX = 0;
                pivotX = 1;
            }

            if(transform.position.y < itemInfo.itemInfoRect.sizeDelta.y)
            {
                anchorY = 0;
                pivotY = 0;
            }
            else
            {
                anchorY = 1;
                pivotY = 1;
            }

            setItenInfoRect(anchorX, anchorY, pivotX, pivotY);
            itemInfo.ShowItemInfo(SlotItem);
        }

    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        if (itemInfo.gameObject.activeSelf)
            itemInfo.HideItemInfo();
    }

    private void setItenInfoRect(float anchorX, float anchorY, float  pivotX, float pivotY)
    {
        //앵커와 피봇 위치 잡기 쉽게 슬롯의 자식으로 잠깐 설정
        itemInfo.itemInfoRect.SetParent(transform);

        //앵커 설정
        itemInfo.itemInfoRect.anchorMin = new Vector2(anchorX, anchorY);
        itemInfo.itemInfoRect.anchorMax = new Vector2(anchorX, anchorY);

        //피봇 설정 및 포지션을 (0, 0)으로 초기화
        itemInfo.itemInfoRect.pivot = new Vector2(pivotX, pivotY);
        itemInfo.itemInfoRect.anchoredPosition = Vector2.zero;

        //앵커, 피봇 설정이 끝났기 때문에 다시 캔버스 자식으로 두고 가장 앞에 보이게 함
        itemInfo.itemInfoRect.SetParent(uiManager.transform);
        itemInfo.itemInfoRect.SetAsLastSibling();
    }
}
