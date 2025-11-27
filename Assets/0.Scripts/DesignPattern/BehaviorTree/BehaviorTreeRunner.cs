public class BehaviorTreeRunner
{
    //루트 노드, 행동 트리의 시작 노드
    private INode rootNode;

    public BehaviorTreeRunner(INode rootNode)
    {
        this.rootNode = rootNode;
    }

    public void Operate()
    {
        rootNode.Evaluate();
    }
}
