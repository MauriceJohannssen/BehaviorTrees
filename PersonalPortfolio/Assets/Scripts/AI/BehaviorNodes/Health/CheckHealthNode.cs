using System.IO.Compression;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

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
        if (_AI.hidingFirstTime)
        {
            if (_AI.CurrentHealth <= _criticalHealthThreshold) nodeState = State.Success;
        }
        else
        {
            if (_AI.CurrentHealth < _AI.initialHealth) nodeState = State.Success;
            else
            {
                _AI.hidingFirstTime = true;
                nodeState = State.Failure;
            }
        }

        return nodeState;
    }
}
 