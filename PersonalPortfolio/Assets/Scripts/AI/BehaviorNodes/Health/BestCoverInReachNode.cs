using UnityEngine;
using System.Collections.Generic;

public class BestCoverInReachNode : Node
{
    private List<GameObject> _hidePositions;
    private AIBlackboard _AI;

    public BestCoverInReachNode(List<GameObject> hidePositions, AIBlackboard AI)
    {
        _hidePositions = hidePositions;
        _AI = AI;
    }

    public override State EvaluateState()
    {
        float currentShortestHideSpot = float.PositiveInfinity;
        foreach (var possiblePosition in _AI.GetAllHideableObject())
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