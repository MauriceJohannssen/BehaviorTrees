using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverInReach : Node
{
    private Transform[] _hidePositions;
    private GameObject _enemy;
    private AIBlackboard _AI;

    public CoverInReach(Transform[] hidePositions, GameObject enemy, AIBlackboard AI)
    {
        _hidePositions = hidePositions;
        _enemy = enemy;
        _AI = AI;
    }

    public override State EvaluateState()
    {
        if (!_AI.NavAgent.isStopped && _AI.AIstate == AIState.Hide) return State.Running;
        
        foreach (var possiblePosition in _hidePositions)
        {
            if (Vector3.Distance(possiblePosition.position, _AI.transform.position) <= _AI.HideRadius)
            {
                //Do proper position comparisons here
                _AI.coverSpot = possiblePosition.position;
                return State.Success;
            }
        }

        return State.Failure;
    }
}
