using UnityEngine;

public class CurrentlyCovered : Node
{
    private AIBlackboard _AI;

    public CurrentlyCovered(AIBlackboard AI)
    {
        _AI = AI;
    }

    public override State EvaluateState()
    {
        if (Physics.Linecast(_AI.transform.position + new Vector3(0,0.5f,0), _AI.Player.transform.position))
        {
            //If it hits something, it must be any but the player, since the player is being ignored
            _AI.isCovered = true;
            nodeState = State.Success;
        }
        else nodeState = State.Failure;
        return nodeState;
    }
}