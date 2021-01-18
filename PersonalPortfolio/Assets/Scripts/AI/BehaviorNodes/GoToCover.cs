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
        if (_AI.AIstate != AIState.Hide)
        {
            _AI.NavAgent.destination = _AI.currentCoverSpot;
            _AI.NavAgent.isStopped = false;
            _AI.AIstate = AIState.Hide;
            nodeState = State.Success;
            return nodeState;
        }
        else if (_AI.AIstate == AIState.Hide && _AI.NavAgent.remainingDistance > 0.2f)
        {
            nodeState = State.Running;
            return nodeState;
        }

        nodeState = State.Failure;
        return nodeState;
    }
}