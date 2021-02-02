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
        nodeState = _AI.WasShot ? State.Success : State.Failure;
        _AI.LastKnownPosition = _AI.Player.transform.position;
        return nodeState;
    }
}
