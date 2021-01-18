using System.Collections;
using System.Collections.Generic;
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
            _AI.NavAgent.destination = _AI.coverSpot;
            _AI.NavAgent.isStopped = false;
            _AI.AIstate = AIState.Hide;
            Debug.Log("Cover spot is " + _AI.coverSpot);
            return State.Success;
        }

        if (_AI.NavAgent.remainingDistance > 0.2f)
        {
            Debug.Log("Going to cover");
            return State.Running;
        }
        
        Debug.Log("Go to cover evaluated to failure");
        return State.Failure;
    }
}