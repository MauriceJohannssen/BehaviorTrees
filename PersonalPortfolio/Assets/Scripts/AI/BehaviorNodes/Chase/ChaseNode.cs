using UnityEngine;

public class ChaseNode : Node
{
    private AIBlackboard _AI;

    public ChaseNode(AIBlackboard AI)
    {
        _AI = AI; 
    }

    public override State EvaluateState()
    {
        _AI.NavAgent.isStopped = false;
        _AI.NavAgent.SetDestination(_AI.LastKnownPosition);
        //Can this even be a failure?
        nodeState = State.Success;
        return nodeState;
    }
}