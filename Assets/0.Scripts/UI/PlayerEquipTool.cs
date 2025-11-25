using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 플레이어가 들고 있는 도구를 UI 창에 보여주기 위한 클래스
/// </summary>
public class PlayerEquipTool : MonoBehaviour
{
    [SerializeField] private Image toolImg;

    public void ShowEquipTool(Tool tool)
    {
        if (toolImg == null) return;

        if(tool == null)
        {
            //도구가 없을 경우 → 맨손일 경우, 이미지 알파값을 0으로 만들어 안보이게 만들기
            toolImg.sprite = null;
            Color color = new Color(1, 1, 1, 0);
            toolImg.color = color;
        }
        else
        {
            //도구가 있을 경우, 해당 도구 이미지를 띄우기
            toolImg.sprite = tool.toolSprite;
            Color color = new Color(1, 1, 1, 1);
            toolImg.color = color;
        }
    }
}
