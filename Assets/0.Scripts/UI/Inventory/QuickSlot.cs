using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : Slot
{
    [SerializeField] private Image selectIcon;

    //퀵슬롯에 있는 템을 플레이어도 들고있게 하기
    public void SelectSlot()
    {
        GameManager.Instance.Player.CurrentSlot = this;
        selectIcon.gameObject.SetActive(true);

    }

    public void DeselectSlot()
    {
        GameManager.Instance.Player.CurrentSlot = null;
        selectIcon.gameObject.SetActive(false);
    }
}
