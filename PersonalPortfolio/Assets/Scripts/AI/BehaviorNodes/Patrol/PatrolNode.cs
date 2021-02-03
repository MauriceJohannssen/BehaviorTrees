using UnityEngine;

public class PatrolNode : Node
{
    private AIBlackboard _AI;
    private Vector3 _nextOffset;
    private Vector3 _currentPosition;
    private float _timeToGoToPosition;
    private float _currentTime;
    private Vector3 _minimumOffset = Vector3.one * 5;  

    public PatrolNode(AIBlackboard AI)
    {
        _AI = AI;
        _currentPosition = _AI.transform.position;
        _nextOffset = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)) * 10;
        _timeToGoToPosition = Random.Range(3.0f, 5.0f);
        _currentTime = 0;
    }

    public override State EvaluateState()
    {
        _AI.NavAgent.isStopped = false;
        
        if (_currentTime > _timeToGoToPosition)
        {
            _currentPosition = _nextOffset;
            if(Random.Range(0,2) == 0) _nextOffset = _minimumOffset + new Vector3(Random.Range(0.0f, 1.0f), 0, Random.Range(0.0f, 1.0f)) * 8;
            else _nextOffset = -_minimumOffset + new Vector3(Random.Range(-1.0f, 0.0f), 0, Random.Range(-1.0f, 0.0f)) * 8;
            _timeToGoToPosition = Random.Range(3.0f, 5.0f);
            _currentTime = 0;
        }
        
        _currentTime += Time.deltaTime;

        Vector3 interpolatedPosition = Vector3.Lerp(_currentPosition, _nextOffset, _currentTime / _timeToGoToPosition);

        _AI.NavAgent.SetDestination(_AI.GetBoss().transform.position + interpolatedPosition);
        return State.Success;
    }
}