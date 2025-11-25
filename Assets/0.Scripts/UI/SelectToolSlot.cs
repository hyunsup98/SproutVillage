using UnityEngine;
using UnityEngine.UI;

public class SelectToolSlot : MonoBehaviour
{
    [SerializeField] private SelectToolGroup selectToolGroup;
    [SerializeField] private Toggle toggleSlot;                         
    [SerializeField] private Image slotImg;         //선택 슬롯의 Image
    [SerializeField] private Image itemImg;         //아이템을 보여줄 Image
    [SerializeField] private Sprite toolSprite;     //아이템 스프라이트
    [SerializeField] private Tool tool;

    [SerializeField] private PlayerEquipTool playerEquipTool;

    private void Awake()
    {
        if (slotImg == null)
            TryGetComponent(out slotImg);

        if (toggleSlot == null)
            TryGetComponent(out toggleSlot);

        itemImg.sprite = toolSprite;
    }

    public void OnValueChanged()
    {
        //토글이 isOn인 슬롯만 실행하기 위함
        if (!toggleSlot.isOn) return;

        //플레이어의 도구 바꾸기
        if (GameManager.Instance.Player != null)
            GameManager.Instance.Player.SetPlayerTool(tool);

        //도구 선택 UI 가리기
        if(selectToolGroup != null)
            selectToolGroup.HideSelectToolGroup();

        //플레이어 현재 도구를 보여주는 UI 갱신
        if (playerEquipTool != null)
            playerEquipTool.ShowEquipTool(tool);
    }
}
