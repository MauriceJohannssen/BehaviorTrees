using System.Timers;
using UnityEngine;
using UnityEngine.AI;

public class RotateToFindPlayerNode : Node
{
    private AIBlackboard _AI;
    private float _lookOutForPlayerTime;
    private float _currentMovementTime = 0;
    private float _currentPauseTime = 0;
    private float _rotationSpeed;

    public RotateToFindPlayerNode(AIBlackboard AI)
    {
        _AI = AI;
        _lookOutForPlayerTime = _AI.LookOutForPlayerTime;
        _rotationSpeed = Random.Range(-130.0f, 130.0f);
    }

    public override State EvaluateState()
    {
        if (_AI.SawPlayer)
        {
            if (_AI.NavAgent.remainingDistance >= 0.5f) nodeState = State.Running;
            else if (_lookOutForPlayerTime > 0 && _AI.NavAgent.remainingDistance <= 0.5f)
            {
                if (_currentMovementTime <= 2.0f)
                {
                    _AI.transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
                    _currentMovementTime += Time.deltaTime;
                    _currentPauseTime = 0;
                }
                else if (_currentPauseTime <= 0.75f)
                {
                    _currentPauseTime += Time.deltaTime;
                }
                else
                {
                    _currentMovementTime = 0;
                    _rotationSpeed = Random.Range(-130.0f, 130.0f);
                }
                
                
                _lookOutForPlayerTime -= Time.deltaTime;
                nodeState = State.Running;
            }
            else
            {
                _AI.SawPlayer = false;
                _lookOutForPlayerTime = _AI.LookOutForPlayerTime;
                nodeState = State.Failure;
            }
        }

        return nodeState;
    }
}