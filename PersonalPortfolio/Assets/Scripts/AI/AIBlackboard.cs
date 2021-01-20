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
    [HideInInspector] public bool hidingFirstTime;

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
        hidingFirstTime = true;
        _currentHealth = initialHealth;
        NavAgent = GetComponent<NavMeshAgent>();
        NavAgent.isStopped = true;
        _player = GameObject.FindWithTag("Player");
        CreateBehaviorTree();
    }

    private void CreateBehaviorTree()
    {
        //Reposition
        IsAtCoverNode isAtCoverNode = new IsAtCoverNode(this);

        SequenceNode repositionSequence = new SequenceNode(new List<Node> {isAtCoverNode});


        //HidingFirstTime Sequence
        GoToCover goToCoverNode = new GoToCover(this);
        BestCoverInReachNode bestCoverInReachNode = new BestCoverInReachNode(hidePositions, this);
        SequenceNode goToReachableCoverSequence =
            new SequenceNode(new List<Node> {bestCoverInReachNode, goToCoverNode});
        HidingFirstTimeNode hidingFirstTimeNode = new HidingFirstTimeNode(this);
        SequenceNode isHidingFirstTimeSequence =
            new SequenceNode(new List<Node> {hidingFirstTimeNode, goToReachableCoverSequence});


        SelectorNode hideFirstTimeSelector =
            new SelectorNode(new List<Node> {isHidingFirstTimeSequence, repositionSequence});

        IsCoverReachableNode isCoverReachableNode = new IsCoverReachableNode(hidePositions, this);

        SequenceNode isCoverReachableSequence =
            new SequenceNode(new List<Node> {isCoverReachableNode, hideFirstTimeSelector});

        //Attack node here

        //Done until here
        SelectorNode coverInReachSelector = new SelectorNode(new List<Node> {isCoverReachableSequence});

        CurrentlyCovered currentlyCoveredNode = new CurrentlyCovered(this, _player.transform);

        SelectorNode coverSelector = new SelectorNode(new List<Node> {currentlyCoveredNode, coverInReachSelector});

        CheckHealthNode checkHealthNode = new CheckHealthNode(this, criticalHealthThreshold);
        SequenceNode healthSequenceNode = new SequenceNode(new List<Node> {checkHealthNode, coverSelector});

        //Attack=======================================================================================================
        AttackNode attackNode = new AttackNode(this, _player.transform);
        PlayerInReachNode playerInReachNode = new PlayerInReachNode(this, _player.transform);
        SequenceNode attackSequenceNode = new SequenceNode(new List<Node> {playerInReachNode, attackNode});

        //Chase========================================================================================================
        ChaseNode chaseNode = new ChaseNode(this, _player.transform);
        SequenceNode chasePlayerSequence = new SequenceNode(new List<Node> {chaseNode});

        //Main=========================================================================================================
        _mainNode = new SelectorNode(new List<Node> {healthSequenceNode, attackSequenceNode, chasePlayerSequence});
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
        //Debug.Log("Current state is " + AIstate);
    }
}

public enum AIState
{
    Hide,
    Hidden,
    Reposition,
    Attack,
    Chase
}