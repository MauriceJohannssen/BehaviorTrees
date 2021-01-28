﻿using UnityEngine;

public class GoToCover : Node
{
    private AIBlackboard _AI;
    
    public GoToCover(AIBlackboard AI)
    {
        _AI = AI;
    }

    public override State EvaluateState()
    {
        Vector3 objectRight = _AI.currentCoverObject.transform.right;
        Vector3 objectCenterToPlayer = Vector3.Normalize(_AI.Player.transform.position - _AI.currentCoverObject.transform.position);
        float angle = Vector3.Angle(objectRight, objectCenterToPlayer);
        
        float cross = Vector3.Cross(objectRight, objectCenterToPlayer).y;
        if (cross < 0.0f)
        {
            angle = 360.0f - angle;
        }

        float finalAngle = 180 - angle;
        
        Debug.Log("Angle was " + finalAngle);

        finalAngle *= Mathf.Deg2Rad;
        _AI.AngleToHideableObject = finalAngle;
        Vector3 bestPosition = new Vector3(Mathf.Cos(finalAngle), 0, Mathf.Sin(finalAngle)) * 4; /* * (4 + Random.Range(0.0f, 0.75f)); */
        
        _AI.NavAgent.destination = _AI.currentCoverObject.transform.position + bestPosition;
        _AI.NavAgent.isStopped = false;
        //Can is even be false?
        nodeState = State.Success;
        return nodeState;
    }   
}