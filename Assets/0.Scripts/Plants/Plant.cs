using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Plant : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer plantRenderer;         
    
    [SerializeField] protected Sprite[] sprites;                //각 단계 별 Sprite
    [SerializeField] protected float growthDuration = 60f;      //다음 단계로 자랄 때 까지 걸리는 시간(초)
    [SerializeField] private Item item;

    public bool canHarvest { get; private set; }                //다 자랐을 경우 true
    private bool canInteract;

    protected Vector3Int plantPos = Vector3Int.zero;

    protected Coroutine growCoroutine;      //식물이 자라는 메서드를 담는 코루틴

    private void Awake()
    {
        if (plantRenderer == null)
            TryGetComponent(out plantRenderer);
    }

    private void Update()
    {
        if (canInteract && canHarvest)
        {
            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                var i = ItemPool.Instance.GetObjects(item, ItemPool.Instance.transform);
                UIManager.Instance.InvenScripts.AddItem(i);
                PlantPool.Instance.TakeObjects(this);
                UIManager.Instance.EnabledInteractableText(false);
            }
        }
    }

    public void Init(Vector3Int pos)
    {
        plantPos = pos;
    }

    //작물 성장 코루틴
    protected IEnumerator Grow()
    {
        float timer = 0f;
        int growIndex = 0;                      //현재 작물 성장 단계
        int maxGrowIndex = sprites.Length - 1;  //최대 작물 성장 단계

        //작물이 최대 성장 단계까지 자라기 전
        while (growIndex < maxGrowIndex)
        {
            yield return CoroutineManager.waitForSeconds(1f);

            if (CanGrow())
            {
                //타이머 1초 올리기
                timer++;

                //타이머가 성장 시간보다 커지면 단계 성장
                if(timer >= growthDuration)
                {
                    timer = 0f;

                    //growIndex를 먼저 올리고 Sprite 반영
                    plantRenderer.sprite = sprites[++growIndex];
                }

                //작물이 성장을 다 할 경우
                if(growIndex == maxGrowIndex)
                {
                    //수확 가능한 상태로 변경
                    canHarvest = true;

                    //코루틴 중지
                    growCoroutine = null;
                    yield break;
                }
            }
        }
    }

    //작물이 자랄 수 있는지 체크하는 메서드
    protected bool CanGrow()
    {
        //현재 내 위치에 젖은 타일이 있을 경우 true 반환 → 자랄 수 있는 환경
        if (TileManager.Instance.GetTile(TileManager.Instance.wetSoilTilemap, plantPos) != null)
            return true;

        //젖은 타일이 없을 경우 false 반환 → 자랄 수 없는 환경
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && canHarvest)
        {
            canInteract = true;
            UIManager.Instance.EnabledInteractableText(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canHarvest)
        {
            canInteract = false;
            UIManager.Instance.EnabledInteractableText(false);
        }
    }

    protected virtual void OnEnable()
    {
        if (growCoroutine == null)
            growCoroutine = StartCoroutine(Grow());
    }

    protected virtual void OnDisable()
    {
        canHarvest = false;
        canInteract = false;
        plantPos = Vector3Int.zero;
        plantRenderer.sprite = sprites[0];

        if (growCoroutine != null)
        {
            StopCoroutine(growCoroutine);
            growCoroutine = null;
        }
    }
}
