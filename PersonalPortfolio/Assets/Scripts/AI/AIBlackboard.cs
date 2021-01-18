using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBlackboard : MonoBehaviour
{
    //Health
    [SerializeField] private float initialHealth = 100.0f;
    [SerializeField] private float healRatePerSecond = 5.0f;
    private float _healTimer = 0;
    private float _currentHealth;
    public float CurrentHealth => _currentHealth;
    [SerializeField] private float criticalHealthThreshold = 20.0f;

    //Hide
    [SerializeField] private float hideRadius = 30.0f;
    public float HideRadius => hideRadius;
    [SerializeField] public Transform[] hidePositions;
    [HideInInspector] public Vector3 currentCoverSpot;
    [HideInInspector] public bool isCovered;
    
    //Attack
    [SerializeField] private float sightRadius = 5.0f;
    public float SightRadius => sightRadius;
    [SerializeField] private float shootInterval = 1.0f;
    public float ShootInterval => shootInterval;

    //NavAgent
    public NavMeshAgent NavAgent { get; private set; }
    [HideInInspector] public AIState AIstate; //Do I really need this?

    //Player
    private GameObject _player;
    public GameObject Player => _player;
    
    //Root node
    private SelectorNode _mainNode;

    private void Start()
    {
        _currentHealth = initialHealth;
        NavAgent = GetComponent<NavMeshAgent>();
        NavAgent.isStopped = true;
        _player = GameObject.FindWithTag("Player");
        CreateBehaviorTree();
    }
    private void CreateBehaviorTree()
    {
        //Health
        CoverInReach coverInReachNode = new CoverInReach(hidePositions, this);
        GoToCover goToCover = new GoToCover(this);
        SequenceNode goToReachableCoverSequence = new SequenceNode(new List<Node> {coverInReachNode, goToCover});
        
        SelectorNode coverInReachSelector = new SelectorNode(new List<Node> {goToReachableCoverSequence});
        
        CurrentlyCovered currentlyCoveredNode = new CurrentlyCovered(this, _player.transform);
        
        SelectorNode coverSelector = new SelectorNode(new List<Node>{currentlyCoveredNode, coverInReachSelector});
        
        CheckHealthNode checkHealthNode = new CheckHealthNode(this, criticalHealthThreshold);
        SequenceNode healthSequenceNode = new SequenceNode(new List<Node> { checkHealthNode, coverSelector });
        
        
        //Attack
        AttackNode attackNode = new AttackNode(this, _player.transform);
        PlayerInReachNode playerInReachNode = new PlayerInReachNode(this, _player.transform);
        SequenceNode attackSequenceNode = new SequenceNode(new List<Node> {playerInReachNode, attackNode});
        
        ChaseNode chaseNode = new ChaseNode(this, _player.transform);
        SequenceNode chasePlayerNode = new SequenceNode(new List<Node> {new InverterNode(playerInReachNode), chaseNode});
        
        _mainNode = new SelectorNode(new List<Node> { healthSequenceNode, attackSequenceNode, chasePlayerNode });
    }

    public void ReduceHealth(float damage) 
    {
        if (damage < 0) return;
        _currentHealth -= damage;
    } 

    private void RegenerateHealth()
    {
        if (isCovered)
        {
            _healTimer += Time.deltaTime;
            if (_healTimer >= 1)
            {
                _currentHealth += healRatePerSecond;
                _healTimer = 0;
            }
        }
    }

    private void Update()
    {
        isCovered = false;
        _mainNode.EvaluateState();
        RegenerateHealth();
        Debug.Log("Current state is " + AIstate);
    }
}

public enum AIState
{
    Hide,
    Attack,
    Chase
}
