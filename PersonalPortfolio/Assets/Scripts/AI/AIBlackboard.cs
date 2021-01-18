using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBlackboard : MonoBehaviour
{
    //Health
    [SerializeField]
    private float initialHealth = 100.0f;
    private float currentHealth;
    public float CurrentHealth => currentHealth;
    [SerializeField]
    private float criticalHealthThreshold = 20.0f;

    //Hide
    public float HideRadius { get; private set; }
    public Transform[] hidePositions;
    public Vector3 coverSpot;
    public bool IsCovered;
    
    //Attack
    public float SightRadius = 5.0f;
    public float ShootInterval = 1.0f;
    public float timeSinceLastShot = 0;

    //NavAgent
    public NavMeshAgent NavAgent { get; private set; }
    public AIState AIstate;

    public GameObject Player { get; private set; }

    SelectorNode MainNode = null;

    void Start()
    {
        currentHealth = initialHealth;
        //Why can this only be done here?
        HideRadius = 10000.0f;
        Player = GameObject.FindWithTag("Player");
        NavAgent = GetComponent<NavMeshAgent>();
        NavAgent.isStopped = true;
        CreateBehaviorTree();
    }
    private void CreateBehaviorTree()
    {
        //Health
        CoverInReach coverInReachNode = new CoverInReach(hidePositions, gameObject, this);
        GoToCover goToCover = new GoToCover(this);
        SequenceNode goToReachableCover = new SequenceNode(new List<Node> {coverInReachNode, goToCover});

        //Add attack or smth here as well
        SelectorNode coverInReach = new SelectorNode(new List<Node> {goToReachableCover});

        
        CurrentlyCovered currentlyCoveredNode = new CurrentlyCovered(this, Player.transform);
        
        SelectorNode cover = new SelectorNode(new List<Node>{currentlyCoveredNode, coverInReach});
        
        CheckHealthNode checkHealthNode = new CheckHealthNode(this, criticalHealthThreshold);
        SequenceNode healthSequenceNode = new SequenceNode(new List<Node> { checkHealthNode, cover });
        
        
        //Attack
        AttackNode attackNode = new AttackNode(this, Player.transform);
        PlayerInReachNode playerInReachNode = new PlayerInReachNode(this, Player.transform);
        SequenceNode attackSequenceNode = new SequenceNode(new List<Node> {playerInReachNode, attackNode});
        
        //PlayerInReachNode playerInReachNode = new PlayerInReachNode(this, Player.transform);
        ChaseNode chaseNode = new ChaseNode(this, Player.transform);
        SequenceNode ChasePlayerNode = new SequenceNode(new List<Node> {new InverterNode(playerInReachNode), chaseNode});
        
        MainNode = new SelectorNode(new List<Node> { healthSequenceNode, attackSequenceNode, ChasePlayerNode });
    }

    public void ReduceHealth(float damage)
    {
        if (damage < 0) return;
        currentHealth -= damage;
    }

    private void RegenerateHealth()
    {
        if(IsCovered) currentHealth += 0.5f;
    }

    private void Update()
    {
        IsCovered = false;
        MainNode.EvaluateState();
        Debug.Log(currentHealth);
        RegenerateHealth();
    }
}

public enum AIState
{
    Hide,
    Attack
}
