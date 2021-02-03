using UnityEngine;
using System.Collections.Generic;

public class IsCoverReachableNode : Node
{
    private AIBlackboard _AI;

    public IsCoverReachableNode(AIBlackboard AI)
    {
        _AI = AI;
    }

    public override State EvaluateState()
    {
        // float currentShortestHideSpot = float.PositiveInfinity;
        // foreach (var possiblePosition in _hidePositions)
        // {
        //     Debug.Log("Position was evaluated");
        //     float distanceToHideSpot = Vector3.Distance(possiblePosition.position, _AI.transform.position);
        //     if (distanceToHideSpot <= _AI.HideRadius && distanceToHideSpot < currentShortestHideSpot)
        //     {
        //         _AI.currentCoverSpot = possiblePosition.position;
        //         currentShortestHideSpot = distanceToHideSpot;
        //     }
        // }
        //
        //nodeState = currentShortestHideSpot < float.PositiveInfinity ? State.Success : State.Failure;
        //return nodeState;

        foreach (var possiblePosition in _AI.GetHideableObjects())
        {
            if (Vector3.Distance(possiblePosition.transform.position, _AI.transform.position) <= _AI.HideRadius)
            {
                nodeState = State.Success;
            }
            else nodeState = State.Failure;
        }
        return nodeState;
    }
}
