using UnityEngine;

public class ChaseNode : Node
{
    private AIBlackboard _AI;
    private Transform _player;

    public ChaseNode(AIBlackboard AI, Transform player)
    {
        _AI = AI;
        _player = player;
    }

    public override State EvaluateState()
    {
        _AI.NavAgent.isStopped = false;
        _AI.NavAgent.SetDestination(_player.position);

        //Can this even be a failure?
        nodeState = State.Success;
        return nodeState;
    }
}