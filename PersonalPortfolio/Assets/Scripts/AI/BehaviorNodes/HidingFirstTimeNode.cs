using UnityEngine;

public class HidingFirstTimeNode : Node
{
    private AIBlackboard _AI;
    private bool test = true;

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

        Debug.Log("Hiding first time was: " + nodeState);
        return nodeState;
    }
}