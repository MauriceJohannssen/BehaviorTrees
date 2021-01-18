using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNode : Node
{
    private AIBlackboard AI;
    private Transform _player;
    public AttackNode(AIBlackboard AI, Transform player)
    {
        this.AI = AI;
        _player = player;
    }

    public override State EvaluateState()
    {
        if (AI.timeSinceLastShot >= AI.ShootInterval)
        {
            AI.GetComponent<EnemyWeapon>().Shoot();
            AI.timeSinceLastShot = 0;
        }

        AI.timeSinceLastShot += Time.deltaTime;
        
        AI.AIstate = AIState.Attack;
        Debug.Log("attack boi");
        return State.Success;
    }
}
