using System.Collections.Generic;

public class SequenceNode : Node
{
    private List<Node> nodes = new List<Node>();

    public SequenceNode(List<Node> nodes)
    {
        this.nodes = nodes;
    }

    public override State EvaluateState()
    {
        bool anyNodeRunning = false;

        foreach (Node node in nodes)
        {
            switch (node.EvaluateState())
            {
                case State.Running:
                    anyNodeRunning = true;
                    break;

                case State.Success:
                    //Do nothing
                    break;

                case State.Failure:
                    //If a failure occures during the sequence the whole sequence stops
                    nodeState = State.Failure;
                    return nodeState;

                default:
                    break;
            }
        }

        nodeState = anyNodeRunning ? State.Running : State.Success;
        return nodeState;
    }
}
