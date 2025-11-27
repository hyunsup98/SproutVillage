using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] protected RectTransform slotTrans;       //현재 슬롯의 RectTransform
    [SerializeField] protected UIManager uiManager;           //캔버스의 총괄을 맡는 UIManager 클래스
    [SerializeField] protected GraphicRaycaster gr;

    [field: SerializeField] public Item SlotItem { get; protected set; }              //슬롯에 존재하는 아이템

    [SerializeField] protected Image itemImage;
    [SerializeField] protected TMP_Text countText;

    [SerializeField] protected DragSlot dragSlot;

    private void Awake()
    {
        if (slotTrans == null)
            TryGetComponent(out slotTrans);

        if (uiManager == null)
            transform.root.TryGetComponent(out uiManager);

        if (gr == null)
            transform.root.TryGetComponent(out gr);
    }

    //슬롯에 마우스 포인터가 올라갔을 때
    public virtual void OnPointerEnter(PointerEventData eventData) { }

    //슬롯에서 마우스 포인터가 빠져나올 때
    public virtual void OnPointerExit(PointerEventData eventData) { }

    //드래그를 시작했을 때
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (SlotItem != null)
            {
                //현재 슬롯의 정보를 드래그 슬롯에 옮겨주기
                dragSlot.SetDragSlot(this);
                dragSlot.transform.position = eventData.position;

                //일단 현재 슬롯의 정보 감추기
                countText.text = string.Empty;
                SetColor(0);
            }
        }
    }

    //드래그를 하는 중일 때
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            dragSlot.transform.position = eventData.position;
        }
    }

    //드래그가 끝났을 때
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (SlotItem != null)
            {
                //드래그를 끝낸 뒤에도 아이템이 있을 경우 슬롯 이미지 보여주고 개수 체크
                SetColor(1);
                CheckItemCount();

                var ped = uiManager.pointerEventData;
                ped.position = eventData.position;
                List<RaycastResult> results = new List<RaycastResult>();
                gr.Raycast(ped, results);

                Slot slot = null;

                foreach (var result in results)
                {
                    if (result.gameObject.CompareTag("Slot"))
                    {
                        slot = result.gameObject.GetComponent<Slot>();
                        continue;
                    }
                }

                if (slot == null)
                {
                    //아이템 파괴 UI 띄우기
                }
            }

            dragSlot.ClearDragSlot();
        }
    }

    //슬롯 위에서 무언가 드랍되었을 때, 내가 드래그하는 상황이 아님
    public virtual void OnDrop(PointerEventData eventData)
    {
        //드래그 슬롯이 null이 아니고, 현재 슬롯이 아닐 때
        if (dragSlot.refSlot != null && dragSlot.refSlot != this)
        {
            Item dragItem = dragSlot.refSlot.SlotItem;

            //현재 슬롯에도 아이템이 있으면
            if (SlotItem != null)
            {
                if (SlotItem.itemData.ItemId != dragItem.itemData.ItemId)
                {
                    //현재 슬롯 아이템과 드래그 슬롯 아이템읟 종류가 다르면 아이템 정보를 크로스
                    dragSlot.refSlot.UpdateSlotData(SlotItem);
                }
                else
                {
                    //현재 슬롯 아이템과 드래그 슬롯 아이템읟 종류가 같으면 개수만 올리기
                    int remain = SlotItem.itemData.maxCount - SlotItem.Count;

                    if (dragItem.Count <= remain)
                    {
                        //드래그 슬롯의 아이템 개수가 현재 슬롯 아이템이랑 한 번에 합쳐지면 합치고 드래그 슬롯 템 정보 제거
                        SlotItem.Count += dragItem.Count;
                        dragSlot.refSlot.ClearSlot();
                    }
                    else
                    {
                        //개수가 더 많으면 여기에 다 합치고 남은건 드래그 슬롯이 참조하는 슬롯에 저장
                        SlotItem.Count = SlotItem.itemData.maxCount;
                        dragItem.Count = dragItem.Count - remain;

                        dragSlot.refSlot.UpdateSlotData(dragItem);
                    }

                    UpdateSlotData();
                    return;
                }
            }
            else  //현재 슬롯에 아이템이 없으면
            {
                dragSlot.refSlot.ClearSlot();
            }

            UpdateSlotData(dragItem);
        }
    }

    //슬롯에 아이템이 있을 때 슬롯 정보 갱신
    public void UpdateSlotData()
    {
        if (SlotItem == null) return;
        if (SlotItem.Count <= 0)
        {
            ClearSlot();
            return;
        }


        SetCountText();
        SetColor(1);
    }

    //슬롯에 아이템을 넣고 슬롯 정보 갱신
    public void UpdateSlotData(Item item)
    {
        SlotItem = item;
        itemImage.sprite = SlotItem.itemData.ItemSprite;

        SetCountText();
        SetColor(1);
    }

    //아이템 개수 체크
    private void CheckItemCount()
    {
        if (SlotItem == null || SlotItem.Count <= 0)
        {
            //슬롯에 아이템 없애기
            ItemPool.Instance.TakeObjects(SlotItem);
            ClearSlot();
        }
        else
        {
            UpdateSlotData();
        }
    }

    //아이템이 있을 때 아이템 개수 텍스트 갱신
    private void SetCountText()
    {
        if (SlotItem != null && countText != null)
        {
            if (SlotItem.itemData.IsStackable && SlotItem.Count > 0)
                countText.text = SlotItem.Count.ToString();
            else
                countText.text = string.Empty;
        }
    }

    //현재 슬롯의 아이템 이미지 알파값을 조정
    private void SetColor(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }

    private void ClearSlot()
    {
        if (SlotItem != null)
            SlotItem = null;

        if (itemImage != null)
            itemImage.sprite = null;

        if (countText != null)
            countText.text = string.Empty;

        SetColor(0);
    }
}
