using System.Collections.Generic;

public class SequenceNode : Node
{
    private List<Node> nodes = new List<Node>();
    public SequenceNode(List<Node> nodes)
    {
        this.nodes = nodes;
    }

    public override State EvaluateStatus()
    {
        bool anyNodeRunning = false;

        foreach (Node node in nodes)
        {
            switch (node.EvaluateStatus())
            {
                case State.Running:
                    anyNodeRunning = true;
                    break;

                case State.Success:
                    //Do nothing
                    break;

                case State.Failure:
                    nodeState = State.Failure;
                    return State.Failure;

                default:
                    break;
            }
        }

        nodeState = anyNodeRunning ? State.Running : State.Success;
        return nodeState;
    }
}
