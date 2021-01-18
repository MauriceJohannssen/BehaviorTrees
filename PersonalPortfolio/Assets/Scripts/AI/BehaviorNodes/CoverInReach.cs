using System;
using UnityEngine;

public class CoverInReach : Node
{
    private Transform[] _hidePositions;
    private AIBlackboard _AI;
    private float testValue = 0;

    public CoverInReach(Transform[] hidePositions, AIBlackboard AI)
    {
        _hidePositions = hidePositions;
        _AI = AI;
    }

    public override State EvaluateState()
    {
        if (_AI.AIstate == AIState.Hide)
        {
            //Evaluate better positions here!
            //_AI.currentCoverSpot =_hidePositions[0].position + new Vector3(Mathf.Cos(testValue), 0, Mathf.Sin(testValue)) * 3;
            //_AI.NavAgent.SetDestination(_AI.currentCoverSpot);
            nodeState = State.Running;
            //testValue += 50.0f;
            return nodeState;
        }
        
        float currentShortestHideSpot = float.PositiveInfinity;
        foreach (var possiblePosition in _hidePositions)
        {
            Debug.Log("Position was evaluated");
            float distanceToHideSpot = Vector3.Distance(possiblePosition.position, _AI.transform.position);
            if (distanceToHideSpot <= _AI.HideRadius && distanceToHideSpot < currentShortestHideSpot)
            {
                _AI.currentCoverSpot = possiblePosition.position;
                currentShortestHideSpot = distanceToHideSpot;
            }
        }

        nodeState = currentShortestHideSpot < float.PositiveInfinity ? State.Success : State.Failure;
        return nodeState;
    }
}
