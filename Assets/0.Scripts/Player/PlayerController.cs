using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRigid;
    [SerializeField] private Animator playerAnim;
    [field: SerializeField] public float MoveSpeed { get; private set; } = 2.5f;
    [field : SerializeField] public float RunSpeed { get; private set; } = 4f;
    [field: SerializeField] public float DashForce { get; private set; } = 7f;

    public Vector2 moveDir { get; private set; } = Vector2.zero;
    public bool isSprint { get; private set; }

    private IState currentState;

    private void Awake()
    {
        TryGetComponent(out playerRigid);

        #region 인풋시스템 연결
        OnMove();
        OnRun();
        OnDash();
        #endregion
    }

    private void Start()
    {
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
    private void OnMove()
    {
        InputSystem.actions["Move"].performed += ctx =>
        {
            moveDir = ctx.ReadValue<Vector2>();
        };

        InputSystem.actions["Move"].canceled += ctx =>
        {
            moveDir = Vector2.zero;
        };
    }

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

    private void OnDash()
    {
        InputSystem.actions["Dash"].started += ctx =>
        {
            //대쉬
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

    //애니메이션 변경
    public void SetAnimation(string animName)
    {
        playerAnim.SetTrigger(animName);
    }

    //이동
    public void Move()
    {
        playerRigid.linearVelocity = isSprint ? RunSpeed * moveDir : MoveSpeed * moveDir;
        playerAnim.SetFloat("moveX", moveDir.x);
        playerAnim.SetFloat("moveY", moveDir.y);
    }
}
