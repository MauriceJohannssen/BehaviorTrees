public class CurrentCoverValid : Node
{
    private AIBlackboard _AI;

    public CurrentCoverValid(AIBlackboard AI)
    {
        _AI = AI;
    }

    public override State EvaluateState()
    {
        if (_AI.CurrentCoverObject == null) return State.Failure;
        
        if (_AI.CurrentCoverObject.GetComponent<ValidateHideableObject>().HighestPoint.y >=
            _AI.transform.position.y + (_AI.transform.lossyScale.y / 2))
        {
            nodeState = State.Success;
        }
        else
        {
            _AI.RemoveHideableObject(_AI.CurrentCoverObject);
            nodeState = State.Failure;
        }

        return nodeState;
    }
}
