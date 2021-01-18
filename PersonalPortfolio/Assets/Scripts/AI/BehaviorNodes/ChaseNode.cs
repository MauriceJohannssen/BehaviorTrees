using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseNode : Node
{
    private AIBlackboard AI;
    private Transform _player;

    public ChaseNode(AIBlackboard AI, Transform player)
    {
        this.AI = AI;
        _player = player;
    }

    public override State EvaluateState()
    {
        AI.NavAgent.isStopped = false;
        AI.NavAgent.SetDestination(_player.position);
        AI.AIstate = AIState.Attack;
        Debug.Log("attack boi");
        return State.Success;
    }
}