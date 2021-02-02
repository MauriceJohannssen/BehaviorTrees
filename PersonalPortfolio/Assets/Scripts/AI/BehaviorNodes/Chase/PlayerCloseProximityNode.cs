using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCloseProximityNode : Node
{
    private AIBlackboard _AI;
    
    public PlayerCloseProximityNode(AIBlackboard AI)
    {
        _AI = AI;
    }

    public override State EvaluateState()
    {
        if (Vector3.Distance(_AI.transform.position, _AI.Player.transform.position) <= 5)
        {
            _AI.LastKnownPosition = _AI.Player.transform.position;
            nodeState = State.Success;
        }
        else nodeState = State.Failure;
        
        Debug.Log("Player in close proximity evaluated to " + nodeState);

        return nodeState;
    }
}
