using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GoToBoss : Node
{
    private AIBlackboard _AI;
    public GoToBoss(AIBlackboard AI)
    {
        _AI = AI;
    }

    public override State EvaluateState()
    {
        // Vector3 newPosition = _AI.GetBoss().transform.position + new Vector3(Mathf.Cos(Random.Range(0.0f, 2 * Mathf.PI)), 0,
        //     Mathf.Sin(Random.Range(0.0f, 2 * Mathf.PI))) * 4;
        // Debug.Log("Destination was set");

        Vector3 normalizedBossToBossDestination =
            (_AI.GetBoss().GetComponent<NavMeshAgent>().destination - _AI.GetBoss().transform.position).normalized;

        Vector3 cross = -Vector3.Cross(_AI.GetBoss().transform.forward, Vector3.up);

        Vector3 bossToEnemy = _AI.transform.position - _AI.GetBoss().transform.position;

        float testPos = Vector3.Dot(cross, bossToEnemy);

        Vector3 finalPosition = _AI.GetBoss().transform.position + cross * testPos;

        Vector3 newPosition = finalPosition + normalizedBossToBossDestination * 10.0f;
        
        Debug.Log("new pos was at" + newPosition);
     
        _AI.NavAgent.isStopped = false;
        _AI.NavAgent.SetDestination(newPosition);
        _AI.walkingToBoss = true;
        nodeState = State.Success;
        return nodeState;
    }
}

