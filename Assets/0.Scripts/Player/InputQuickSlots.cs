using UnityEngine;
using UnityEngine.InputSystem;

public class InputQuickSlots : MonoBehaviour
{
    [SerializeField] private QuickSlot[] quickSlots;
    private int prevSlotIndex = int.MaxValue;       //이전에 눌렀던 키를 또 누를 경우 아이템을 해제하기 위해 기억해둘 변수

    private void Awake()
    {
        OnQuickSlots();
    }

    public void OnQuickSlots()
    {
        InputSystem.actions["QuickSlot"].started += ctx =>
        {
            if(GameManager.Instance.CurrentGameState == GameState.Playing)
            {
                string key = ctx.control.name;  //어떤 키를 입력했는지 key에 넣어줌

                if (int.TryParse(key, out var slotIndex))
                {
                    //배열 인덱스에 접근하기 때문에 내가 누른 키 -1
                    slotIndex--;

                    //누른 키에 해당하는 퀵슬롯이 존재할 때
                    if (slotIndex >= 0 && slotIndex < quickSlots.Length)
                    {
                        if (slotIndex == prevSlotIndex)
                        {
                            //이전에 눌렀던 퀵슬롯일 때
                            prevSlotIndex = int.MaxValue;
                            quickSlots[slotIndex].DeselectSlot();
                        }
                        else
                        {
                            //새로 누르는 키일 때
                            if (prevSlotIndex != int.MaxValue)
                                quickSlots[prevSlotIndex].DeselectSlot();

                            prevSlotIndex = slotIndex;
                            quickSlots[slotIndex].SelectSlot();
                        }
                    }
                }
            }
        };
    }
}
