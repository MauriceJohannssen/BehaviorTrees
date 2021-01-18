public class CheckHealthNode : Node
{
    private AIBlackboard _AI;
    private float _criticalHealthThreshold;

    public CheckHealthNode(AIBlackboard AI, float criticalHealthThreshold)
    {
        _AI = AI;
        _criticalHealthThreshold = criticalHealthThreshold;
    }

    public override State EvaluateState()
    {
        nodeState = _AI.CurrentHealth >= _criticalHealthThreshold ? State.Failure : State.Success;
        return nodeState;
    }
}
