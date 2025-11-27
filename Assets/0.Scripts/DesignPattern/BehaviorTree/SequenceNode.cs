using System.Collections.Generic;

public class SequenceNode : INode
{
    private List<INode> childs;

    public SequenceNode(List<INode> childs)
    {
        this.childs = childs;
    }

    public INode.ENodeState Evaluate()
    {
        if (childs == null)
            return INode.ENodeState.ENS_Failure;

        foreach(var child in childs)
        {
            var state = child.Evaluate();

            if (state != INode.ENodeState.ENS_Success)
                return state;
        }

        return INode.ENodeState.ENS_Success;
    }
}
