using UnityEngine;

public class CurrentlyCovered : Node
{
    private AIBlackboard _AI;
    private Transform _player;

    public CurrentlyCovered(AIBlackboard AI, Transform player)
    {
        _AI = AI;
        _player = player;
    }

    public override State EvaluateState()
    {
        if (Physics.Linecast(_AI.transform.position, _player.transform.position))
        {
            //If it hits something, it must be any but the player, since the player is being ignored
            _AI.isCovered = true;
            //_AI.AIstate = AIState.Hidden;
            nodeState = State.Success;
            return nodeState;
        }

        nodeState = State.Failure;
        return nodeState;
    }
}