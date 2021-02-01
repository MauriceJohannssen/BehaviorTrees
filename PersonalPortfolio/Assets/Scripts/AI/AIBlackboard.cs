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
    [SerializeField] private float criticalHealthThreshold = 20.0f;
    private float _healTimer;
    private float _currentHealth;
    public float CurrentHealth => _currentHealth;

    //Hide
    [SerializeField] private float hideRadius = 30.0f;
    public float HideRadius => hideRadius;
    private List<GameObject> _hidePositions;

    private GameObject _currentCoverObject;
    public GameObject CurrentCoverObject
    {
        get => _currentCoverObject;
        set
        {
            if (value.tag.Equals("Hideable")) _currentCoverObject = value;
            else Debug.LogError("You tried to set a non-hideable object as object to cover!");
        }
    }

    [HideInInspector] public bool isCovered;
    [HideInInspector] public bool hidingFirstTime;
    [HideInInspector] public float angleToHideableObject;

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
    
    //Boss
    private GameObject _boss;
    public bool walkingToBoss = false;

    //Root node
    private SelectorNode _mainNode;

    private void Start()
    {
        InitializeVariables();
        GetAllHideables();
        FindBoss();
        FindPlayer();
        CreateBehaviorTree();
    }

    private void CreateBehaviorTree()
    {
        //Boss=========================================================================================================
        GoingToBossNode goingToBossNode = new GoingToBossNode(this);
        
        IsInRange isBossInRangeNode = new IsInRange(this);
        GoToBoss goToBossNode = new GoToBoss(this);
        
        SequenceNode followBossSequence = new SequenceNode(new List<Node>{new InverterNode(isBossInRangeNode), goToBossNode});

        SelectorNode currentBossBehaviour = new SelectorNode(new List<Node> {goingToBossNode, followBossSequence});
        
        //Health=======================================================================================================
        //Reposition sequence
        
        //Reposition
        RepositionNode repositionNode = new RepositionNode(this);
        IsAtCoverNode isAtCoverNode = new IsAtCoverNode(this);
        //RepositionNode placed here
        SequenceNode repositionSequence = new SequenceNode(new List<Node> {isAtCoverNode, new InverterNode(repositionNode)});
        
        //IsHidingFirstTime sequence
        GoToCover goToCoverNode = new GoToCover(this);
        BestCoverInReachNode bestCoverInReachNode = new BestCoverInReachNode(_hidePositions, this);
        SequenceNode goToReachableCoverSequence = new SequenceNode(new List<Node> {bestCoverInReachNode, goToCoverNode});
        IsCoverReachableNode isCoverReachableNode = new IsCoverReachableNode(_hidePositions, this);
        SequenceNode isCoverReachableSequence = new SequenceNode(new List<Node> {isCoverReachableNode, goToReachableCoverSequence});

        CurrentCoverValid currentCoverValidNode = new CurrentCoverValid(this);
        HidingFirstTimeNode hidingFirstTimeNode = new HidingFirstTimeNode(this);

        SelectorNode iDunno = new SelectorNode(new List<Node> {hidingFirstTimeNode, new InverterNode(currentCoverValidNode)});
        
        SequenceNode isHidingFirstTimeSequence = new SequenceNode(new List<Node> {iDunno, isCoverReachableSequence});
        
        //CurrentlyCovered "Sequence"
        CurrentlyCovered currentlyCoveredNode = new CurrentlyCovered(this, _player.transform);

        SelectorNode coverSelector = new SelectorNode(new List<Node> {currentlyCoveredNode, isHidingFirstTimeSequence, new InverterNode(repositionSequence)});

        IsValidCoverLeftNode isValidCoverLeftNode = new IsValidCoverLeftNode(this);
        
        CheckHealthNode checkHealthNode = new CheckHealthNode(this, criticalHealthThreshold);
        SequenceNode healthSequenceNode = new SequenceNode(new List<Node> {checkHealthNode, isValidCoverLeftNode, coverSelector});

        //Attack=======================================================================================================
        AttackNode attackNode = new AttackNode(this, _player.transform);
        PlayerInReachNode playerInReachNode = new PlayerInReachNode(this, _player.transform);
        SequenceNode attackSequenceNode = new SequenceNode(new List<Node> {playerInReachNode, attackNode});

        //Chase========================================================================================================
        ChaseNode chaseNode = new ChaseNode(this, _player.transform);
        SequenceNode chasePlayerSequence = new SequenceNode(new List<Node> {chaseNode});

        //Main=========================================================================================================
        _mainNode = new SelectorNode(new List<Node> {healthSequenceNode, currentBossBehaviour, attackSequenceNode, chasePlayerSequence});
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
            _hidePositions.Add(hideableObject);
        }
    }

    public void RemoveHideableObject(GameObject objectToRemove)
    {
        _hidePositions.Remove(objectToRemove);
    }

    public List<GameObject> GetAllHideableObject()
    {
        return _hidePositions;
    }

    private void FindBoss()
    {
        _boss = GameObject.FindWithTag("Boss");
    }

    public GameObject GetBoss()
    {
        return _boss;
    }

    private void FindPlayer()
    {
        _player = GameObject.FindWithTag("Player");
    }

    private void InitializeVariables()
    {
        _hidePositions = new List<GameObject>();
        hidingFirstTime = true;
        _currentHealth = initialHealth;
        NavAgent = GetComponent<NavMeshAgent>();
        NavAgent.isStopped = true;
    }
}