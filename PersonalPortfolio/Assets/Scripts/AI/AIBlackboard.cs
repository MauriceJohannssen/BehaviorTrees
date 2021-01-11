using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBlackboard : MonoBehaviour
{
    [SerializeField]
    private float initialHealth = 100.0f;
    private float currentHealth;

    [SerializeField]
    private float criticalHealthThreshold = 20.0f;

    public float HideRadius { get; private set; }

    public Transform[] hidePositions;

    public float CurrentHealth => currentHealth;
    public Vector3 coverSpot;

    public NavMeshAgent NavAgent { get; private set; }

    SelectorNode MainNode = null;

    void Start()
    {
        currentHealth = initialHealth;
        //Why can this only be done here?
        HideRadius = 10000.0f;
        NavAgent = GetComponent<NavMeshAgent>();
        NavAgent.isStopped = true;
        CreateBehaviorTree();
    }

    private void CreateBehaviorTree()
    {
        CoverInReach coverInReachNode = new CoverInReach(hidePositions, gameObject, this);
        GoToCover goToCover = new GoToCover(this);
        SequenceNode goToReachableCover = new SequenceNode(new List<Node> {coverInReachNode, goToCover});

        //Add attack or smth here as well
        SelectorNode coverInReach = new SelectorNode(new List<Node> {goToReachableCover});

        CheckHealthNode checkHealthNode = new CheckHealthNode(this, criticalHealthThreshold);
        CurrentlyCovered currentlyCoveredNode = new CurrentlyCovered(this);
        
        SelectorNode cover = new SelectorNode(new List<Node>{currentlyCoveredNode, coverInReach});
        
        SequenceNode health = new SequenceNode(new List<Node> { checkHealthNode, cover });
        
        MainNode = new SelectorNode(new List<Node> { health });
    }

    public void ReduceHealth(float damage)
    {
        if (damage < 0) return;
        currentHealth -= damage;
    }

    private void Update()
    {
        MainNode.EvaluateState();
        Debug.Log(currentHealth);
    }
}
