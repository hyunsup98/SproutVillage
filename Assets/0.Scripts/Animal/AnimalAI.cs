using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalAI : MonoBehaviour
{
    [Header("컴포넌트 관련 필드")]
    [SerializeField] private Animator animalAnim;
    [SerializeField] private SpriteRenderer animalRenderer;

    [Header("재료 생산 관련 변수")]
    [SerializeField] private Item item;                     //생산할 아이템
    [SerializeField] private float harvestTime = 300f;      //동물이 특정 재료를 생산하기까지 걸리는 시간
    private float harvestTimer = 0f;                        //재료 생산 타이머

    [Header("스탯 관련 변수")]
    [SerializeField] private float moveSpeed = 2.5f;               //이동 속도
    private bool isMoved;

    private AStar aStar;                                    //에이스타 알고리즘 클래스
    private List<Vector3> path = new List<Vector3>();       //이동 경로
    private Vector3 destination = Vector3.zero;             //지금 당장의 목적지
    private float pathUpdateTimer = float.MaxValue;                     //경로 갱신을 위한 타이머
    private bool isCalling;                                 //플레이어가 동물을 부른 상태인지에 대한 여부, true → 불렀음 false → 안불렀음

    //행동 트리
    private BehaviorTreeRunner BTRunner;

    private void IsPlayerCallling(bool isCalling) => this.isCalling = isCalling;

    private void Awake()
    {
        BTRunner = new BehaviorTreeRunner(SettingBT());
        aStar = new AStar();

        moveSpeed = Random.Range(moveSpeed - 1f, moveSpeed + 1f);
    }

    private void Start()
    {
        if (GameManager.Instance.Player != null)
        {
            GameManager.Instance.Player.onCalledAnimals += IsPlayerCallling;
        }
    }

    private void Update()
    {
        BTRunner.Operate();
    }

    private INode SettingBT()
    {
        return new SelectorNode
            (
                new List<INode>()
                {
                    new SequenceNode
                    (
                        new List<INode>()
                        {
                            new ActionNode(CanHarvest),
                            new ActionNode(ProductionItem),
                        }
                    ),
                    new SequenceNode
                    (
                        new List<INode>()
                        {
                            new ActionNode(IsPlayerCalling),
                            new ActionNode(FollowPlayer),
                        }
                    ),
                }
            );
    }

    #region 행동 트리에서 사용할 액션 노드 메서드들
    //생산할 수 있는지 체크하는 메서드
    private INode.ENodeState CanHarvest()
    {
        harvestTimer += Time.deltaTime;

        if(harvestTimer >= harvestTime)
        {
            //생산 시간이 되면 타이머를 초기화 후 Success 반환
            harvestTimer = 0f;
            return INode.ENodeState.ENS_Success;
        }

        //아직 생산 시간이 안되었기 때문에 Failure 반환
        return INode.ENodeState.ENS_Failure;
    }

    //생산이 가능해져서 재료를 생산하는 메서드
    private INode.ENodeState ProductionItem()
    {
        //재료를 생산 후 설정
        var animalItem = ItemPool.Instance.GetObjects(item, ItemPool.Instance.transform);
        float x = Random.Range(0.0f, 1.0f);
        float y = Random.Range(0.0f, 1.0f);

        animalItem.transform.position = new Vector3(transform.position.x + x, transform.position.y + y, 0f);

        return INode.ENodeState.ENS_Success;
    }

    //플레이어가 동물을 불렀는지 체크하는 메서드
    private INode.ENodeState IsPlayerCalling()
    {
        if (isCalling)
            return INode.ENodeState.ENS_Success;

        animalAnim.SetBool("isMove", false);

        //안불렀으니 Failure 반환
        return INode.ENodeState.ENS_Failure;
    }

    //플레이어를 찾아가는 메서드
    private INode.ENodeState FollowPlayer()
    {
        pathUpdateTimer += Time.deltaTime;

        //경로 갱신 시간이 되면 경로를 다시 갱신해줌
        if(pathUpdateTimer >= 0.5f)
        {
            pathUpdateTimer = 0f;

            path = aStar.FindPath(transform.position, GameManager.Instance.Player.transform.position);

            if (path != null)
            {
                isMoved = true;

                destination = path[0];
                path.RemoveAt(0);
            }
        }
        else if (isMoved && path != null)
        {
            animalAnim.SetBool("isMove", true);
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

            animalRenderer.flipX = transform.position.x < destination.x ? false : true;

            if (Vector3.Distance(transform.position, destination) < 0.5f)
            {
                if (path.Count == 0)
                {
                    isMoved = false;
                }
                else
                {
                    destination = path[0];
                    path.RemoveAt(0);
                }
            }
        }

        return INode.ENodeState.ENS_Success;
    }
    #endregion


    private void OnEnable()
    {
        if (GameManager.Instance.Player != null)
            GameManager.Instance.Player.onCalledAnimals += IsPlayerCallling;
    }

    private void OnDisable()
    {
        if (GameManager.Instance.Player != null)
            GameManager.Instance.Player.onCalledAnimals -= IsPlayerCallling;
    }
}
