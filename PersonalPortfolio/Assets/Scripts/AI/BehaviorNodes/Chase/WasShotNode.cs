using UnityEngine;

public class WasShotNode : Node
{
    private AIBlackboard _AI;
    private bool wasShot = false;
    
    public WasShotNode(AIBlackboard AI)
    {
        _AI = AI;
    }

    public override State EvaluateState()
    {
        if (_AI.WasShot)
        {
            Debug.Log("Was shot at");
            _AI.NavAgent.isStopped = false;
            wasShot = true;
            _AI.SawPlayer = true;
            _AI.LastKnownPosition = _AI.Player.transform.position;
            nodeState = State.Success;
        }
        else if (wasShot)
        {
            if (_AI.NavAgent.remainingDistance > 1.0f)
            {
                nodeState = State.Running;
            }
            else wasShot = false;
        }
        else nodeState = State.Failure;
        return nodeState;
    }
}
