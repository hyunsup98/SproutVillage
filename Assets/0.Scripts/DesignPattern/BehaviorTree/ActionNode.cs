using System;
using UnityEngine;

public class ActionNode : INode
{
    private Func<INode.ENodeState> onUpdate = null;

    public ActionNode(Func<INode.ENodeState> onUpdate)
    {
        this.onUpdate = onUpdate;
    }

    //반환값 타입을 널러블로 했기 때문에 ?? 연산자를 통해 null일 경우 반환 타입을 맞춰줘야됨
    public INode.ENodeState Evaluate() => onUpdate?.Invoke() ?? INode.ENodeState.ENS_Failure;
}
