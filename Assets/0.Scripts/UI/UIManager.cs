using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Canvas canvas;

    public PointerEventData pointerEventData { get; private set; }              //슬롯의 EndDrag에서 사용할 PointerEveneData 캐시

    [field: SerializeField] public Inventory InvenScripts { get; private set; }     //인벤토리 클래스
    [field: SerializeField] public GameObject inventory { get; private set; }       //인벤토리 게임오브젝트, invenScripts와 오브젝트가 다름
    [SerializeField] private GameObject itemInfo;

    [Header("플레이어 소지 골드 텍스트")]
    [SerializeField] private TMP_Text goldText;

    [Header("상호작용 텍스트")]
    [SerializeField] private TMP_Text interactableText;

    public void EnabledInteractableText(bool isEnabled) => interactableText.gameObject.SetActive(isEnabled);

    protected override void Awake()
    {
        base.Awake();

        pointerEventData = new PointerEventData(null);

        OnInventory();
    }

    public void SetGoldText(int gold)
    {
        if(goldText != null)
            goldText.text = gold.ToString();
    }

    private void OnEnable()
    {
        GameManager.Instance.onGameStateTitle += () => 
        { 
            if (canvas.enabled)
                canvas.enabled = false;
        };

        GameManager.Instance.onGameStatePlaying += () =>
        {
            if (!canvas.enabled)
                canvas.enabled = true;
        };
    }

    #region UI 인풋시스템
    //I키를 눌러 인벤토리를 끄거나 킴
    public void OnInventory()
    {
        InputSystem.actions["Inventory"].started += ctx =>
        {
            if(GameManager.Instance.CurrentGameState != GameState.Title)
            {
                inventory.SetActive(!inventory.activeSelf);
                itemInfo.SetActive(inventory.activeSelf);

                GameManager.Instance.CurrentGameState = inventory.activeSelf ? GameState.InteractionLock : GameState.Playing;

                InvenScripts.transform.SetAsLastSibling();
            }
        };
    }
    #endregion
}
