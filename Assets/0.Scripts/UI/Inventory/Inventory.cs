using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    [field: SerializeField] public List<Slot> slots { get; private set; }

    /// <summary>
    /// 아이템 추가
    /// 1. 같은 아이템이 인벤에 있는지 확인 후 있으면 개수 중첩
    /// 2. 없으면 빈칸 슬롯을 찾아 추가
    /// </summary>
    /// <param name="item"> 추가할 아이템 </param>
    public bool AddItem(Item item)
    {
        //인벤에 같은 아이템이 있는 곳에 먼저 추가해줌
        Item tempItem = AddExistItem(item);

        //아직도 개수가 남아있다면 빈 슬롯에 추가해줌
        if(tempItem != null)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].SlotItem == null)
                {
                    slots[i].UpdateSlotData(tempItem);
                    return true;
                }
            }

            return false;
        }

        return true;
    }

    /// <summary>
    /// 이미 인벤토리에 같은 아이템이 존재할 경우 개수 추가 후 남은 만큼 반환
    /// 반환값이 null이면 더 이상 추가할 아이템이 없다는 것, 반환값이 있으면 해당 아이템 개수만큼 더 추가해주면 됨
    /// </summary>
    /// <param name="item"> 추가할 아이템 </param>
    /// <returns> 개수 추가 후 남은 아이템 → 빈 슬롯에 새로 추가할 것 </returns>
    private Item AddExistItem(Item item)
    {
        Item tempItem = item;

        for(int i = 0; i < slots.Count; i++)
        {
            //슬롯의 아이템
            Item slotItem = slots[i].SlotItem;

            //슬롯의 아이템과 추가할 아이템이 같을 때
            if(slotItem != null && slotItem.itemData.ItemId == tempItem.itemData.ItemId)
            {
                //슬롯의 아이템이 스택형이고 최대 개수보다 적을 때
                if(slotItem.itemData.IsStackable && slotItem.Count < slotItem.itemData.maxCount)
                {
                    //현재 슬롯에 추가할 수 있는 개수
                    int remainCount = slotItem.itemData.maxCount - slotItem.Count;

                    //추가할 아이템 개수가 남아있는 공간보다 클 경우
                    if(tempItem.Count > remainCount)
                    {
                        //현재 슬롯 아이템 최대 개수로 만들고 그만큼 tempItem 개수에서 차감
                        slotItem.Count = slotItem.itemData.maxCount;
                        tempItem.Count -= remainCount;
                        slots[i].UpdateSlotData();
                    }
                    else
                    {
                        slotItem.Count += tempItem.Count;
                        slots[i].UpdateSlotData();
                        tempItem.Count = 0;
                        return null;
                    }
                }
            }
        }

        return tempItem;
    }
}
