using UnityEngine;

public class IsAtCoverNode : Node
{
    private AIBlackboard _AI;
    
    public IsAtCoverNode(AIBlackboard AI)
    {
        _AI = AI;
    }

    public override State EvaluateState()
    {
        nodeState = _AI.NavAgent.remainingDistance < 0.5f ? State.Success : State.Failure;
        Debug.Log("IsAtCover is being executed!");
        return nodeState;
    }
}