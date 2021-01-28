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
    private List<GameObject> hidePositions = new List<GameObject>();
    public GameObject currentCoverObject;
    [HideInInspector] public bool isCovered;
    [HideInInspector] public bool hidingFirstTime;
    [HideInInspector] public float AngleToHideableObject = 0;

        //Attack
    [SerializeField] private float sightRadius = 5.0f;
    public float SightRadius => sightRadius;
    [SerializeField] private float shootInterval = 1.0f;
    public float ShootInterval => shootInterval;

    //NavAgent
    public NavMeshAgent NavAgent { get; private set; }

    //Player
    private GameObject _player;
    public GameObject Player => _player;

    //Root node
    private SelectorNode _mainNode;

    private void Start()
    {
        hidingFirstTime = true;
        _currentHealth = initialHealth;
        GetAllHideables();
        NavAgent = GetComponent<NavMeshAgent>();
        NavAgent.isStopped = true;
        _player = GameObject.FindWithTag("Player");
        CreateBehaviorTree();
    }

    private void CreateBehaviorTree()
    {
        //Health=======================================================================================================
        //Reposition sequence
        
        //Reposition
        RepositionNode repositionNode = new RepositionNode(this);
        IsAtCoverNode isAtCoverNode = new IsAtCoverNode(this);
        //RepositionNode placed here
        SequenceNode repositionSequence = new SequenceNode(new List<Node> {isAtCoverNode, new InverterNode(repositionNode)});
        
        //IsHidingFirstTime sequence
        GoToCover goToCoverNode = new GoToCover(this);
        BestCoverInReachNode bestCoverInReachNode = new BestCoverInReachNode(hidePositions, this);
        SequenceNode goToReachableCoverSequence = new SequenceNode(new List<Node> {bestCoverInReachNode, goToCoverNode});
        IsCoverReachableNode isCoverReachableNode = new IsCoverReachableNode(hidePositions, this);
        SequenceNode isCoverReachableSequence = new SequenceNode(new List<Node> {isCoverReachableNode, goToReachableCoverSequence});
        SelectorNode coverInReachSelector = new SelectorNode(new List<Node> {isCoverReachableSequence /* Attack here as well*/});

        ReassignCover reassignCoverNode = new ReassignCover(this);
        HidingFirstTimeNode hidingFirstTimeNode = new HidingFirstTimeNode(this);

        SelectorNode iDunno = new SelectorNode(new List<Node> {hidingFirstTimeNode, reassignCoverNode});
        
        SequenceNode isHidingFirstTimeSequence = new SequenceNode(new List<Node> {iDunno, coverInReachSelector});
        
        //CurrentlyCovered "Sequence"
        CurrentlyCovered currentlyCoveredNode = new CurrentlyCovered(this, _player.transform);

        SelectorNode coverSelector = new SelectorNode(new List<Node> {currentlyCoveredNode, isHidingFirstTimeSequence, new InverterNode(repositionSequence)});

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
    }

    private void GetAllHideables()
    {
        foreach (var hideableObject in GameObject.FindGameObjectsWithTag("Hideable"))
        {
            hidePositions.Add(hideableObject);
        }
    }

    public void RemoveHideableObject(GameObject objectToRemove)
    {
        hidePositions.Remove(objectToRemove);
    }

    public List<GameObject> GetAllHideableObject()
    {
        return hidePositions;
    }
}