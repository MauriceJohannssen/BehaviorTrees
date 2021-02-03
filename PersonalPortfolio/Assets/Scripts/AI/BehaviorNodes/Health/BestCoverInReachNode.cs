using UnityEngine;
using System.Collections.Generic;

public class BestCoverInReachNode : Node
{
    private AIBlackboard _AI;

    public BestCoverInReachNode(AIBlackboard AI)
    {
        _AI = AI;
    }

    public override State EvaluateState()
    {
        float currentShortestHideSpot = float.PositiveInfinity;
        foreach (var possiblePosition in _AI.GetHideableObjects())
        {
            float distanceToHideSpot = Vector3.Distance(possiblePosition.transform.position, _AI.transform.position);
            if (distanceToHideSpot <= _AI.HideRadius && distanceToHideSpot < currentShortestHideSpot)
            {
                _AI.CurrentCoverObject = possiblePosition;
                currentShortestHideSpot = distanceToHideSpot;
            }
        }
        
        nodeState = currentShortestHideSpot < float.PositiveInfinity ? State.Success : State.Failure;
        return nodeState;
    }
}