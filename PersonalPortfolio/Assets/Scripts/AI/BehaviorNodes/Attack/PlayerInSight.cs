using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInSight : Node
{
    private AIBlackboard _AI;
    
    public PlayerInSight(AIBlackboard AI)
    {
        _AI = AI;
    }

    public override State EvaluateState()
    {
        Vector3 enemyToPlayer = _AI.Player.transform.position - _AI.transform.position;
        float angle = Vector3.Angle(_AI.transform.forward, new Vector3(enemyToPlayer.x, 0, enemyToPlayer.z));
        nodeState = angle <= _AI.SightRangeAngle ? State.Success : State.Failure;
        if (angle <= _AI.SightRangeAngle && enemyToPlayer.magnitude <= _AI.SightRadius)
        {
            nodeState = Physics.Linecast(_AI.transform.position, _AI.Player.transform.position)
                ? State.Failure
                : State.Success;

            if (Physics.Linecast(_AI.transform.position, _AI.Player.transform.position))
            {
                nodeState = State.Failure;
            }
            else
            {
                _AI.LastKnownPosition = _AI.Player.transform.position;
                _AI.SawPlayer = true;
                //_AI.transform.LookAt(_AI.Player.transform.position);
                _AI.transform.rotation = Quaternion.RotateTowards(_AI.transform.rotation, Quaternion.LookRotation(enemyToPlayer), 2);
                nodeState = State.Success;
            }
        }
        else nodeState = State.Failure;
        return nodeState;
    }
}
