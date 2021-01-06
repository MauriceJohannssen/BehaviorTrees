using System.Collections.Generic;

public class SelectorNode : Node
{
    private List<Node> nodes = new List<Node>();

    public SelectorNode(List<Node> nodes)
    {
        this.nodes = nodes;
    }

    public override State EvaluateStatus()
    {
        foreach (Node node in nodes)
        {
            switch (node.EvaluateStatus())
            {
                case State.Running:
                    nodeState = State.Running;
                    return nodeState;

                case State.Success:
                    nodeState = State.Success;
                    return nodeState;

                case State.Failure:
                    //Do nothing
                    break;

                default:
                    break;
            }
        }

        nodeState = State.Failure;
        return nodeState;
    }
}
