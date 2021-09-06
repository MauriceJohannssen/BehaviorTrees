public class InverterNode : Node
{
    Node node;

    public InverterNode(Node node)
    {
        this.node = node;
    }

    public override State EvaluateState()
    {
        switch (node.EvaluateState())
        {
            case State.Running:
                //Nothing's changing here!
                nodeState = State.Running;
                break;

            case State.Success:
                nodeState = State.Failure;
                break;

            case State.Failure:
                nodeState = State.Success;
                break;

            default:
                break;
        }

        return nodeState;
    }
}


