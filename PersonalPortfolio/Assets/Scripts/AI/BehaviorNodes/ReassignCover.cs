using System.Collections;
using UnityEngine;

public class ReassignCover : Node
{
    private AIBlackboard _AI;

    public ReassignCover(AIBlackboard AI)
    {
        _AI = AI;
    }

    public override State EvaluateState()
    {
        if (_AI.currentCoverObject == null) return State.Failure;
        
        if (_AI.currentCoverObject.GetComponent<ValidateHideableObject>().HighestPoint.y >=
            _AI.transform.position.y + (_AI.transform.lossyScale.y / 2))
        {
            nodeState = State.Failure;
        }
        else
        {
            _AI.RemoveHideableObject(_AI.currentCoverObject);
            nodeState = State.Success;
            Debug.Log("Object was lower!");
        }

        return nodeState;
    }
}
