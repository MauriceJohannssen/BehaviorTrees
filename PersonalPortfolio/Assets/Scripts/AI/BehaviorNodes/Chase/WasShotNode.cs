using UnityEngine;

public class WasShotNode : Node
{
    private AIBlackboard _AI;
    
    public WasShotNode(AIBlackboard AI)
    {
        _AI = AI;
    }

    public override State EvaluateState()
    {
        if (_AI.WasShot)
        {
            _AI.LastKnownPosition = _AI.Player.transform.position;
            nodeState = State.Success;
        }
        else nodeState = State.Failure;
        return nodeState;
    }
}
