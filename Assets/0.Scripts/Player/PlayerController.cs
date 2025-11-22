using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRigid;
    [SerializeField] private Animator playerAnim;

    [SerializeField] private Tool[] tools;

    [field: SerializeField] public float MoveSpeed { get; private set; } = 2.5f;
    [field : SerializeField] public float RunSpeed { get; private set; } = 4f;
    [field: SerializeField] public float DashForce { get; private set; } = 7f;

    public Vector2 moveDir { get; private set; } = Vector2.zero;    //현재 입력받은 이동값 → W, A, S, D
    public Vector2 idleDir { get; private set; } = Vector2.zero;    //특정 방향의 idle 애니메이션을 재생시키기 위해 이동중이 아니어도 마지막에 향했던 방향을 기억해둠
    public bool isSprint { get; private set; }              //현재 달리는 중이면 true
    public bool isCanDash { get; private set; } = true;     //대쉬가 가능한 상태 → 현재 대쉬중이 아니고 대쉬 쿨타임이 돌았을 때
    public bool isDash { get; private set; }                //현재 대쉬 중이면 true
    [SerializeField] private float dashTime = 0.1f;         //대쉬 속도로 뛰는 시간
    [SerializeField] private float dashCoolTime = 1.5f;     //대쉬 쿨타임
    private Coroutine dashCoroutine;

    public bool isToolInteracted { get; private set; }      //장비를 착용한 채로 상호작용을 했는지

    private IState currentState;        //현재 플레이어의 상태
    private Tool currentTool;           //현재 플레이어가 들고 있는 도구

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

        #region 인풋시스템 연결
        OnMove();
        OnRun();
        OnDash();
        OnToolInteract();
        #endregion
    }

    private void Start()
    {
        GameManager.Instance.SetPlayer(this);

        //초기 상태 설정
        SetState(new PlayerIdleState(this));
    }

    private void Update()
    {
        currentState?.OnUpdate();
    }

    private void FixedUpdate()
    {
        currentState?.OnFixedUpdate();
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
            if(currentTool != null)
            {
                isToolInteracted = true;
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

    //플레이어가 손에 드는 도구 변경
    public void SetPlayerTool(Tool tool)
    {
        if (currentTool == tool) return;

        currentTool = tool;
        Debug.Log(currentTool);
    }

    //현재 들고있는 도구의 상호작용
    public void ToolActivate()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
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
    #endregion

    //이동
    public void Move()
    {
        playerRigid.linearVelocity = isSprint ? RunSpeed * moveDir : MoveSpeed * moveDir;
        SetAnimBlend(("moveX", moveDir.x), ("moveY", moveDir.y));
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
