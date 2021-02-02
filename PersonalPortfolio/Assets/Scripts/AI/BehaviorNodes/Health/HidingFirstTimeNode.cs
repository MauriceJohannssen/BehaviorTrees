using UnityEngine;

public class HidingFirstTimeNode : Node
{
    private AIBlackboard _AI;

    public HidingFirstTimeNode(AIBlackboard AI)
    {
        _AI = AI;
    }

    public override State EvaluateState()
    {
        if (_AI.hidingFirstTime)
        {
            _AI.hidingFirstTime = false;
            nodeState = State.Success;
        }
        else
        {
            nodeState = State.Failure;
        }

        return nodeState;
    }
}