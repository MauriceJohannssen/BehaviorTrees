using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInReachNode : Node
{
    private Transform _player;
    private AIBlackboard AI;

    public PlayerInReachNode(AIBlackboard AI, Transform player)
    {
        _player = player;
        this.AI = AI;
    }

    public override State EvaluateState()
    {
        Vector3 vectorToPlayer = _player.position - AI.transform.position;
        if (vectorToPlayer.magnitude <= AI.SightRadius)
        {
            return State.Success;
        }

        return State.Failure;
    }
}
