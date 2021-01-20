using UnityEngine;
using System.Collections.Generic;

public class BestCoverInReachNode : Node
{
    private List<Transform> _hidePositions;
    private AIBlackboard _AI;

    public BestCoverInReachNode(List<Transform> hidePositions, AIBlackboard AI)
    {
        _hidePositions = hidePositions;
        _AI = AI;
    }

    public override State EvaluateState()
    {
        Debug.Log("BestCoverInReach");
        float currentShortestHideSpot = float.PositiveInfinity;
        foreach (var possiblePosition in _hidePositions)
        {
            float distanceToHideSpot = Vector3.Distance(possiblePosition.position, _AI.transform.position);
            if (distanceToHideSpot <= _AI.HideRadius && distanceToHideSpot < currentShortestHideSpot)
            {
                _AI.currentCoverSpot = possiblePosition.position;
                currentShortestHideSpot = distanceToHideSpot;
            }
        }
        
        nodeState = currentShortestHideSpot < float.PositiveInfinity ? State.Success : State.Failure;
        Debug.Log(nodeState);
        return nodeState;
    }
}