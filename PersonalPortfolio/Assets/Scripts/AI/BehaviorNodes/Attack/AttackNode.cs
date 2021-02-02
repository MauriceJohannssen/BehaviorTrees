using UnityEngine;

public class AttackNode : Node
{
    private AIBlackboard _AI;
    private Transform _player;
    private float _timeSinceLastShot;
    
    public AttackNode(AIBlackboard AI, Transform player)
    {
        _AI = AI;
        _player = player;
    }

    public override State EvaluateState()
    {
        _AI.NavAgent.isStopped = true;
        
        //This can only be as precise as the times called
        if (_timeSinceLastShot >= _AI.ShootInterval)
        {
            _AI.GetComponent<EnemyWeapon>().Shoot();
            _timeSinceLastShot = 0;
        }
        
        _timeSinceLastShot += Time.deltaTime;

        //Can this even be a failure?
        nodeState = State.Success;
        return nodeState;
    }
}
