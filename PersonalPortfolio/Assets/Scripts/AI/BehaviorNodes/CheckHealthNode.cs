public class CheckHealthNode : Node
{
    AIBlackboard AI;
    float criticalHealthThreshold;

    public CheckHealthNode(AIBlackboard AI, float criticalHealthThreshold)
    {
        this.AI = AI;
        this.criticalHealthThreshold = criticalHealthThreshold;
    }

    public override State EvaluateState()
    {
        nodeState = AI.CurrentHealth >= criticalHealthThreshold ? State.Failure : State.Success;
        return nodeState;
    }
}
