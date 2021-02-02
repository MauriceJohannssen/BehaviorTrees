using UnityEngine;
using UnityEngine.AI;

public class RotateToFindPlayerNode : Node
{
    private AIBlackboard _AI;
    private float rotateTimer = 5.0f;
    
    public RotateToFindPlayerNode(AIBlackboard AI)
    {
        _AI = AI;
    }

    public override State EvaluateState()
    {
        if (_AI.SawPlayer && _AI.NavAgent.remainingDistance >= 0.5f) nodeState = State.Running;
        else if (_AI.SawPlayer && rotateTimer > 0 && _AI.NavAgent.remainingDistance <=0.5f)
        {
            _AI.transform.Rotate(0, 30 * Time.deltaTime,0);
            rotateTimer -= Time.deltaTime;
            nodeState = State.Running;
        }
        else
        {
            _AI.SawPlayer = false;
            rotateTimer = 5.0f;
            nodeState = State.Failure;
        }

        return nodeState;
    }
}
