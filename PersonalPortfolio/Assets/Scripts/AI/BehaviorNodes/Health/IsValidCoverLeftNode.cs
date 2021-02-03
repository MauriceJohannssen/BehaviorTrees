using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsValidCoverLeftNode : Node
{
    private AIBlackboard _AI;
    
    public IsValidCoverLeftNode(AIBlackboard AI)
    {
        _AI = AI;
    }
    
    public override State EvaluateState()
    {
        nodeState = _AI.GetHideableObjects().Count > 0 ? State.Success : State.Failure;
        return nodeState;
    }
}
