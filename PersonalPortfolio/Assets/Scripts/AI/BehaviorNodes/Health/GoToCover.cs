using UnityEngine;

public class GoToCover : Node
{
    private AIBlackboard _AI;
    
    public GoToCover(AIBlackboard AI)
    {
        _AI = AI;
    }

    public override State EvaluateState()
    {
        Vector3 objectRight = _AI.CurrentCoverObject.transform.right;
        Vector3 objectCenterToPlayer = Vector3.Normalize(_AI.Player.transform.position - _AI.CurrentCoverObject.transform.position);
        float angle = Vector3.Angle(objectRight, new Vector3(objectCenterToPlayer.x, 0, objectCenterToPlayer.z));
        
        float cross = Vector3.Cross(objectRight, objectCenterToPlayer).y;
        if (cross < 0.0f)
        {
            angle = 360.0f - angle;
        }

        float finalAngle = 180 - angle;
        
        finalAngle *= Mathf.Deg2Rad;
        _AI.angleToHideableObject = finalAngle;
        //Vector3 objectEulerAngles = _AI.CurrentCoverObject.transform.rotation.eulerAngles;
        Vector3 bestPosition = _AI.CurrentCoverObject.transform.rotation * new Vector3(Mathf.Cos(finalAngle), 0, Mathf.Sin(finalAngle)) * 4; /* * (4 + Random.Range(0.0f, 0.75f)); */
        
        _AI.NavAgent.isStopped = false;
        _AI.NavAgent.destination = _AI.CurrentCoverObject.transform.position + bestPosition;
        //Can is even be false?
        nodeState = State.Success;
        return nodeState;
    }   
}