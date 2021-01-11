using UnityEngine;

public class CheckHealthNode : Node
{
    AIBlackboard AI;
    float criticalHealthThreshold;

    public CheckHealthNode(AIBlackboard AI, float critialHealthThreshold)
    {
        this.AI = AI;
        this.criticalHealthThreshold = critialHealthThreshold;
    }

    public override State EvaluateState()
    {
        nodeState = AI.CurrentHealth <= criticalHealthThreshold ? State.Success : State.Failure;
        return nodeState;
    }
}
