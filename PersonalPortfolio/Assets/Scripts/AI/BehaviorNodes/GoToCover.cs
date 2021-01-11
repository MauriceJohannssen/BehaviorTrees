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
        if (_AI.NavAgent.isStopped)
        {
            _AI.NavAgent.destination = _AI.coverSpot;
            _AI.NavAgent.isStopped = false;
            Debug.Log("Cover was set");
            return State.Success;
        }

        if (!_AI.NavAgent.isStopped && _AI.NavAgent.remainingDistance > 0.2f)
        {
            return State.Running;
        }

        return State.Failure;
    }
}