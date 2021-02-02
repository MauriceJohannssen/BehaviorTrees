using UnityEngine;

public class IsInRange : Node
{
    private AIBlackboard _AI;

    public IsInRange(AIBlackboard AI)
    {
        _AI = AI;
    }

    public override State EvaluateState()
    {
        nodeState = (_AI.GetBoss().transform.position - _AI.transform.position).magnitude <= _AI.MaximumDistanceToBoss
            ? State.Success
            : State.Failure;
        
        Debug.Log("Boss is inside range is " + nodeState);

        return nodeState;
    }
}