using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionNode : Node
{
    private AIBlackboard _AI = null;
    private float startingValue = 0;
    
    public RepositionNode(AIBlackboard AI)
    {
        _AI = AI;
    }

    public override State EvaluateState()
    {
        Vector3 aiToHideableObject = Vector3.Normalize(_AI.transform.position - _AI.currentCoverSpot);
        Vector3 playerToHideableObject = Vector3.Normalize(_AI.Player.transform.position - _AI.currentCoverSpot);
        Vector3 resultingCrossVec = Vector3.Cross(aiToHideableObject, playerToHideableObject);
        
        startingValue += resultingCrossVec.y <= 0 ? -0.2f : 0.2f;
        
        float radius = 4.0f;
        _AI.NavAgent.destination = _AI.currentCoverSpot + (new Vector3(Mathf.Cos(startingValue), 0, Mathf.Sin(startingValue)) * (radius + Random.Range(0.0f,0.75f)));
        return State.Running;
    }
}
