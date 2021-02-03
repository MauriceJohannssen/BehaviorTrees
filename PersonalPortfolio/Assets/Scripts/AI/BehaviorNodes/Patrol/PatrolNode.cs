using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolNode : Node
{
    private AIBlackboard _AI;
    private Vector3 _nextOffset;
    private float _remainAtPositionTime = 2.0f;
    
    public PatrolNode(AIBlackboard AI)
    {
        _AI = AI;
        _nextOffset = new Vector3(Random.Range(-1.0f,1.0f), 0, Random.Range(-1.0f, 1.0f)) * 10;
        //_AI.NavAgent.SetDestination(_AI.GetBoss().transform.position + _nextOffset);
    }

    public override State EvaluateState()
    {
        _AI.NavAgent.isStopped = false;
        
        if (_AI.NavAgent.remainingDistance <= 1.0f)
        {
            if (_remainAtPositionTime <= 0)
            {
                _nextOffset = new Vector3(Random.Range(-1.0f,1.0f), 0, Random.Range(-1.0f, 1.0f)) * 10;
                _remainAtPositionTime = Random.Range(0.5f, 2.0f);
            }

            _remainAtPositionTime -= Time.deltaTime;
        }
        
        _AI.NavAgent.SetDestination(_AI.GetBoss().transform.position + _nextOffset);
        return State.Success;
    }
}
