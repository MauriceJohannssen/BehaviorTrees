using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]

public class Patrol : MonoBehaviour
{
    private GameObject[] _patrolNodes;
    private NavMeshAgent _navMeshAgent;
    private int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetAllPatrolNodes();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        HandlePositions();
    }

    private void GetAllPatrolNodes()
    {
        _patrolNodes = GameObject.FindGameObjectsWithTag("PatrolPoint");
        if(_patrolNodes.Length <= 0) Debug.LogError("No patrol points found");
    }

    private void HandlePositions()
    {
        if (_navMeshAgent.remainingDistance <= 0.5f)
        {
            _navMeshAgent.destination = _patrolNodes[currentIndex++].transform.position;
            if (currentIndex == _patrolNodes.Length) currentIndex = 0;
        }
    }
}
