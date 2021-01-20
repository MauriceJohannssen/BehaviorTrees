using UnityEngine;

public class GoToCover : Node
{
    private AIBlackboard _AI;
    
    public GoToCover(AIBlackboard AI)
    {
        _AI = AI;
    }

    public override State EvaluateState()
    {
        _AI.NavAgent.destination = _AI.currentCoverSpot;
        _AI.NavAgent.isStopped = false;
        //Can is even be false?
        nodeState = State.Success;
        return nodeState;
    }
}