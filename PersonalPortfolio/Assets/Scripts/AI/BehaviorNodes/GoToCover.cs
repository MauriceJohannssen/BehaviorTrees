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
        // //Hits this first time
        // if (_AI.AIstate != AIState.Hide)
        // {
        //     _AI.NavAgent.destination = _AI.currentCoverSpot;
        //     _AI.NavAgent.isStopped = false;
        //     _AI.AIstate = AIState.Hide;
        //     nodeState = State.Success;
        //     return nodeState;
        // }
        // //Hits this while hiding
        // else if (_AI.AIstate == AIState.Hide)
        // {
        //     nodeState = State.Running;
        //     return nodeState;
        // }
        //
        // nodeState = State.Failure;
        // return nodeState;
        
        _AI.NavAgent.destination = _AI.currentCoverSpot;
        _AI.NavAgent.isStopped = false;
        //Can is even be false?
        Debug.Log("Go to cover!");
        nodeState = State.Success;
        return nodeState;
    }
}