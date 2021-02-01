using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoingToBossNode : Node
{
    private AIBlackboard _AI;

    public GoingToBossNode(AIBlackboard AI)
    {
        _AI = AI;
    }

    public override State EvaluateState()
    {
        if (_AI.walkingToBoss)
        {
            if (_AI.NavAgent.remainingDistance >= 0.5f)
            {
                nodeState = State.Running;
            }
            else
            {
                _AI.walkingToBoss = false;
                nodeState = State.Failure;
            }
        }
        else nodeState = State.Failure;
        
        //Debug.Log("Walking to the boss evaluated to " + nodeState);

        return nodeState;
    }
}