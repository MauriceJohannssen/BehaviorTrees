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
        Vector3 objectRight = _AI.currentCoverObject.transform.right;
        Vector3 objectCenterToPlayer = Vector3.Normalize(_AI.Player.transform.position - _AI.currentCoverObject.transform.position);
        float angle = Vector3.Angle(objectRight, objectCenterToPlayer);
        
        float cross = Vector3.Cross(objectRight, objectCenterToPlayer).y;
        if (cross < 0) angle = 360 - angle;
        
        Debug.Log("Angle was " + angle);

        angle *= Mathf.PI / 180;
        _AI.AngleToHideableObject = angle;
        Vector3 bestPosition = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * (4 + Random.Range(0.0f, 0.75f));
        
        _AI.NavAgent.destination = _AI.currentCoverObject.transform.position + bestPosition;
        _AI.NavAgent.isStopped = false;
        //Can is even be false?
        nodeState = State.Success;
        return nodeState;
    }
}