
public abstract class Node
{
    public abstract State EvaluateStatus();
    protected State nodeState;
    public State NodeState => nodeState;
}

public enum State
{
    Success, Failure, Running
}