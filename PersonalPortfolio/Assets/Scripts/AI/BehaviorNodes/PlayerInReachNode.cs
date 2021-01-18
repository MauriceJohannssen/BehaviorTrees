using UnityEngine;

public class PlayerInReachNode : Node
{
    private AIBlackboard _AI;
    private Transform _player;

    public PlayerInReachNode(AIBlackboard AI, Transform player)
    {
        _AI = AI;
        _player = player;
    }

    public override State EvaluateState()
    {
        Vector3 vectorToPlayer = _player.position - _AI.transform.position;
        nodeState = vectorToPlayer.magnitude <= _AI.SightRadius ? State.Success : State.Failure;
        return nodeState;
    }
}
