using UnityEngine;
using UnityEngine.AI;

public class GoToBoss : Node
{
    private AIBlackboard _AI;

    public GoToBoss(AIBlackboard AI)
    {
        _AI = AI;
    }

    public override State EvaluateState()
    {
        // Vector3 newPosition = _AI.GetBoss().transform.position + new Vector3(Mathf.Cos(Random.Range(0.0f, 2 * Mathf.PI)), 0,
        //     Mathf.Sin(Random.Range(0.0f, 2 * Mathf.PI))) * 4;
        // Debug.Log("Destination was set");

        Vector3 normalizedBossToBossDestination =
            (_AI.GetBoss().GetComponent<NavMeshAgent>().destination - _AI.GetBoss().transform.position).normalized;

        Vector3 bossCorrectRight = -Vector3.Cross(normalizedBossToBossDestination, _AI.GetBoss().transform.up);

        Vector3 bossToEnemy = _AI.transform.position - _AI.GetBoss().transform.position;

        float enemyToBossOnCross = Vector3.Dot(bossCorrectRight, bossToEnemy);

        float angle = 0;
        if (enemyToBossOnCross >= 5.0f || enemyToBossOnCross <= -5.0f)
        {
            if (enemyToBossOnCross < 0)
            {
                angle += Random.Range(enemyToBossOnCross, 0);
            }
            else angle += Random.Range(0, enemyToBossOnCross);

            Debug.Log("Angle was " + angle);
            angle *= -2.0f;
        }

        Vector3 newPosition = _AI.GetBoss().transform.position + bossCorrectRight * enemyToBossOnCross;

        Vector3 finalPosition = newPosition + (Quaternion.Euler(0, angle, 0) * normalizedBossToBossDestination) *
            Random.Range(5.0f, 17.5f);

        _AI.NavAgent.isStopped = false;
        _AI.NavAgent.SetDestination(finalPosition);
        _AI.walkingToBoss = true;
        nodeState = State.Success;
        return nodeState;
    }
}