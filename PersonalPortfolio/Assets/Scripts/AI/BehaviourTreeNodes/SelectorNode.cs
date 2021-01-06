using System.Collections.Generic;

public class SelectorNode : Node
{
    private List<Node> nodes = new List<Node>();

    public SelectorNode(List<Node> nodes)
    {
        this.nodes = nodes;
    }

    public override State EvaluateState()
    {
        foreach (Node node in nodes)
        {
            switch (node.EvaluateState())
            {
                case State.Running:
                    nodeState = State.Running;
                    return nodeState;

                case State.Success:
                    nodeState = State.Success;
                    return nodeState;

                case State.Failure:
                    //Do nothing and continue checking the next node
                    break;

                default:
                    break;
            }
        }

        nodeState = State.Failure;
        return nodeState;
    }
}
