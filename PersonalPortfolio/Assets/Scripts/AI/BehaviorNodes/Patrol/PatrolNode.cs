using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolNode : Node
{
    private AIBlackboard _AI;
    
    public PatrolNode(AIBlackboard AI)
    {
        _AI = AI;
    }

    public override State EvaluateState()
    {
        _AI.NavAgent.SetDestination(_AI.GetBoss().transform.position);
        return State.Success;
    }
}
