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
        if (Vector3.Distance(_AI.transform.position, _AI.Player.transform.position) <= _AI.NoticableProximity)
        {
            _AI.LastKnownPosition = _AI.Player.transform.position;
            _AI.SawPlayer = true;
            nodeState = State.Success;
        }
        else nodeState = State.Failure;
        return nodeState;
    }
}
