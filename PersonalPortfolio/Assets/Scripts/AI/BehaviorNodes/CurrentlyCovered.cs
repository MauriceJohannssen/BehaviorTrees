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
        if (Physics.Linecast(_AI.transform.position, _AI.gameObject.transform.position, out RaycastHit raycastHit, 2))
        {
            //If it hits something, it must be any but the player, since the player is being ignored
            return State.Success;
        }
        
        return State.Failure;
    }
}