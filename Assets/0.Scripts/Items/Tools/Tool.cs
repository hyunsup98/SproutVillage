using UnityEngine;

public abstract class Tool : MonoBehaviour
{
    [field : SerializeField] public Sprite toolSprite { get; private set; }     //UI가 필요할 때 띄울 도구 스프라이트
    [field : SerializeField] public float coolTime { get; private set; }         //도구 상호작용 쿨타임

    public PlayerToolState playerToolState { get; protected set; }

    public abstract void SetState(PlayerController player);

    public abstract void Activate();
}
