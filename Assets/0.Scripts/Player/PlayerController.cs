using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("컴포넌트 관련 필드")]
    [SerializeField] private Rigidbody2D playerRigid;
    [SerializeField] private Animator playerAnim;

    [Header("플레이어가 가질 도구들")]
    [SerializeField] private Tool[] tools;

    [Header("물 주는 애니메이션에 사용할 물 애니메이터")]
    [SerializeField] private Animator waterAnim;                //물 주는 애니메이션에서 물을 재생시킬 애니메이터

    [Header("플레이어를 타일 좌표로 변환할 때 사용할 기준점")]
    [SerializeField] private Transform tilePreviewTransform;    //상호작용될 타일을 시각적으로 보여줄 때의 기준점이 될 위치 → 플레이어의 발 밑

    [Header("스탯 설정 값")]
    [field: SerializeField] public float MoveSpeed { get; private set; } = 2.5f;
    [field : SerializeField] public float RunSpeed { get; private set; } = 4f;
    [field: SerializeField] public float DashForce { get; private set; } = 7f;

    public event Action<bool> onCalledAnimals;          //플레이어가 동물을 부르거나 부르지 않을 때 호출할 이벤트
    public bool isCalling { get; private set; }         //true → 플레이어가 동물을 부름 false → 플레이어가 동물을 부르지 않음

    [Header("이동 관련 필드")]
    public Vector2 moveDir { get; private set; } = Vector2.zero;    //현재 입력받은 이동값 → W, A, S, D
    public Vector2 idleDir { get; private set; } = Vector2.down;    //특정 방향의 idle 애니메이션을 재생시키기 위해 이동중이 아니어도 마지막에 향했던 방향을 기억해둠
    public Vector2 cursorDir { get; private set; } = Vector2.zero;  //마우스 커서가 바라보는 방향
    public bool isSprint { get; private set; }              //현재 달리는 중이면 true
    public bool isCanDash { get; private set; } = true;     //대쉬가 가능한 상태 → 현재 대쉬중이 아니고 대쉬 쿨타임이 돌았을 때
    public bool isDash { get; private set; }                //현재 대쉬 중이면 true
    [SerializeField] private float dashTime = 0.1f;         //대쉬 속도로 뛰는 시간
    [SerializeField] private float dashCoolTime = 1.5f;     //대쉬 쿨타임
    private Coroutine dashCoroutine;

    [SerializeField] private AudioClip callAnimalClip;
    [SerializeField] private AudioClip grazeAnimalClip;

    [SerializeField] private int gold = 300;
    public int Gold
    {
        get { return gold; }
        set
        {
            gold = value;
            UIManager.Instance.SetGoldText(gold);
        }
    }

    private bool hasActiveatedAnim;                         //애니메이션 이벤트가 실행되었는지 체크하는 변수, 블렌드 트리에서는 섞인 애니메이션의 이벤트가 모두 발동됨

    public bool isToolInteracted { get; private set; }      //장비를 착용한 채로 상호작용을 했는지

    private Tool currentTool;                               //현재 플레이어가 들고 있는 도구
    public Tool CurrentTool
    {
        get { return currentTool; }
        set
        {
            if (currentTool == value || GameManager.Instance.CurrentGameState != GameState.Playing) return;

            currentTool = value;
        }
    }

    private QuickSlot currentSlot;                               //퀵슬롯을 통해 현재 플레이어가 들고 있는 아이템
    public QuickSlot CurrentSlot
    {
        get { return currentSlot; }
        set
        {
            if (currentSlot == value) return;

            currentSlot = value;
        }
    }

    public IState currentState { get; private set; }                      //현재 플레이어의 상태

    private void Awake()
    {
        TryGetComponent(out playerRigid);

        //플레이어가 들고있는 도구들의 상태 객체 생성
        if(tools.Length > 0)
        {
            foreach(var tool in tools)
            {
                tool.SetState(this);
            }
        }

        GameManager.Instance.Player = this;

        #region 인풋시스템 연결
        OnMove();
        OnRun();
        OnDash();
        OnToolInteract();
        OnItemInteract();
        CallingAnimal();
        #endregion
    }

    private void Start()
    {
        //초기 상태 설정
        SetState(new PlayerIdleState(this));
    }

    private void Update()
    {
        GetTargetTilePosition();

        currentState?.OnUpdate();
    }

    private void FixedUpdate()
    {
        currentState?.OnFixedUpdate();
    }

    private void LateUpdate()
    {
        //애니메이션 이벤트가 실행된 후 LateUpdate로 넘어가기 때문에 여기서 다시 초기화
        if(hasActiveatedAnim)
            hasActiveatedAnim = false;
    }

    #region Input System
    //인풋시스템으로 받아오는 입력에 따른 이벤트들

    //걷기 입력 → W, A, S, D
    private void OnMove()
    {
        InputSystem.actions["Move"].performed += ctx =>
        {
            moveDir = ctx.ReadValue<Vector2>();
            idleDir = moveDir;
        };

        InputSystem.actions["Move"].canceled += ctx =>
        {
            moveDir = Vector2.zero;
        };
    }

    //달리기 입력 → 왼쪽 쉬프트
    private void OnRun()
    {
        InputSystem.actions["Sprint"].performed += ctx =>
        {
            isSprint = true;
        };

        InputSystem.actions["Sprint"].canceled += ctx =>
        {
            isSprint = false;
        };
    }

    //대쉬 입력 → 스페이스바
    private void OnDash()
    {
        InputSystem.actions["Dash"].started += ctx =>
        {
            if (!isDash && isCanDash)
            {
                isDash = true;
                isCanDash = false;
            }
        };
    }

    //도구 상호작용 키 입력 → 왼쪽 마우스버튼
    private void OnToolInteract()
    {
        InputSystem.actions["ToolInteract"].started += ctx =>
        {
            if(GameManager.Instance.CurrentGameState == GameState.Playing && CurrentTool != null)
            {
                isToolInteracted = true;
            }
        };

        InputSystem.actions["ToolInteract"].canceled += ctx =>
        {
            if (CurrentTool != null)
            {
                isToolInteracted = false;
            }
        };
    }

    private void OnItemInteract()
    {
        InputSystem.actions["ItemInteract"].started += ctx =>
        {
            if (GameManager.Instance.CurrentGameState == GameState.Playing && CurrentSlot != null)
            {
                if(CurrentSlot.SlotItem is IUsable)
                {
                    var item = CurrentSlot.SlotItem as IUsable;
                    item.Use(TileManager.Instance.interactTile);
                    CurrentSlot.UpdateSlotData();
                }
            }
        };
    }

    private void CallingAnimal()
    {
        InputSystem.actions["CallingAnimal"].started += ctx =>
        {
            if(GameManager.Instance.CurrentGameState == GameState.Playing)
            {
                isCalling = !isCalling;

                SoundManager.Instance.PlaySoundEffect(isCalling ? callAnimalClip : grazeAnimalClip);
                onCalledAnimals?.Invoke(isCalling);
            }
        };
    }
    #endregion

    //상태 변경
    public void SetState(IState state)
    {
        currentState?.OnExit();
        currentState = state;
        currentState?.OnEnter();
    }

    public void Activate()
    {
        if (CurrentTool == null || hasActiveatedAnim) return;

        hasActiveatedAnim = true;

        CurrentTool.Activate();
    }

    /// <summary>
    /// 마우스와 플레이어의 위치를 통해 플레이어 근처 타일 좌표를 계산
    /// 반경 범위 → 플레이어 주변 한 칸
    /// ■ ■ ■
    /// ■ p ■
    /// ■ ■ ■
    /// </summary>
    private void GetTargetTilePosition()
    {
        if (GameManager.Instance.CurrentGameState != GameState.Playing) return;

        //보정이 끝난 최종 좌표
        Vector3Int result = Vector3Int.zero;

        //마우스 좌표 받아오기
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0;

        //좌표 위치 보정, 플레이어 주변 타일 한칸 까지만 적용되게
        Vector3 pPos = Vector3Int.FloorToInt(tilePreviewTransform.position);
        int pPosX = Mathf.FloorToInt(tilePreviewTransform.position.x);    //플레이어의 현재 x좌표를 타일 좌표로 처리하기 위해 소수점 버림
        int pPosY = Mathf.FloorToInt(tilePreviewTransform.position.y);    //플레이어의 현재 y좌표를 타일 좌표로 처리하기 위해 소수점 버림

        Vector3 dir = mousePos - new Vector3(pPosX, pPosY, 0);

        if (dir.x < 0)
            result.x = pPosX - 1;
        else if (dir.x < 1)
            result.x = pPosX;
        else
            result.x = pPosX + 1;

        if (dir.y < 0)
            result.y = pPosY - 1;
        else if (dir.y < 1)
            result.y = pPosY;
        else
            result.y = pPosY + 1;

        //cursorDir에 현재 마우스 좌표 주기
        cursorDir = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y).normalized;
        TileManager.Instance.ViewTile(result);
    }

    #region 애니메이션 변경
    //Trigger 파라미터를 이용한 애니메이션 변경
    public void SetAnimTrigger(string animName)
    {
        playerAnim.SetTrigger(animName);
    }

    //Bool 파라미터를 이용한 애니메이션 변경
    public void SetAnimBool(string animName, bool isTrue)
    {
        playerAnim.SetBool(animName, isTrue);
    }

    //Blend Tree가 사용중인 파라미터를 이용한 애니메이션 변경
    public void SetAnimBlend(params (string animName, float value)[] blendVal)
    {
        foreach(var b in blendVal)
        {
            playerAnim.SetFloat(b.animName, b.value);
        }
    }

    //물주는 애니메이션, idleDir에 따라 방향 정해주기
    public void SetWaterAnim()
    {
        if (Mathf.Abs(cursorDir.x) > Mathf.Abs(cursorDir.y))
        {
            if(cursorDir.x < 0)
            {
                waterAnim.SetTrigger("left");
            }
            else
            {
                waterAnim.SetTrigger("right");
            }
        }
        else
        {
            if(cursorDir.y < 0)
            {
                waterAnim.SetTrigger("down");
            }
            else
            {
                waterAnim.SetTrigger("up");
            }
        }
    }
    #endregion

    //이동
    public void Move()
    {
        playerRigid.linearVelocity = isSprint ? RunSpeed * moveDir : MoveSpeed * moveDir;
        SetAnimBlend(("moveX", moveDir.x), ("moveY", moveDir.y));
    }

    //이동 상태 끝날 때 움직임을 멈출 메서드
    public void Stop()
    {
        playerRigid.linearVelocity = Vector2.zero;
    }

    public void Dash()
    {
        if (dashCoroutine != null)
            StopCoroutine(dashCoroutine);

        dashCoroutine = StartCoroutine(CoDash());
    }

    private IEnumerator CoDash()
    {
        float timer = 0f;
        float dashCoolRemaining = dashCoolTime - dashTime;

        while (timer < dashTime)
        {
            timer += Time.deltaTime;

            //대쉬하는 도중에 이동키를 뗄 수도 있기 때문에 마지막 수치를 기억하고 있는 idleDir을 사용
            playerRigid.linearVelocity = DashForce * idleDir;

            yield return CoroutineManager.waitForFixedUpdate;
        }

        //실질적인 대쉬는 끝
        isDash = false;

        yield return CoroutineManager.waitForSeconds(dashCoolRemaining);

        //대쉬 쿨타임이 끝나서 대쉬가 가능한 상태로 변경
        isCanDash = true;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
