using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBlackboard : MonoBehaviour
{
    //Health
    [Header("Health")]
    [SerializeField] public float initialHealth = 100.0f;
    [SerializeField] private float healRatePerSecond = 5.0f;
    [SerializeField] private float criticalHealthThreshold = 40.0f;
    private float _healTimer;
    private float _currentHealth;
    public float CurrentHealth => _currentHealth;
    
    //Hide
    [Header("Hiding")]
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
    [Header("Attack")]
    [SerializeField] private float shootRadius = 5.0f;
    public float ShootRadius => shootRadius;
    [SerializeField] private float shootInterval = 1.0f;
    public float ShootInterval => shootInterval;

    //NavAgent
    public NavMeshAgent NavAgent { get; private set; }
    
    //Chase
    [Header("Chase")]
    public float SightRangeAngle = 70.0f;
    public float SightRadius = 30.0f;
    public float NoticableProximity = 3.0f;
    public float LookOutForPlayerTime = 10.0f;
    [HideInInspector]public bool SawPlayer = false;
    public bool WasShot = false;

    //Player
    private GameObject _player;
    public GameObject Player => _player;
    [HideInInspector]public Vector3 LastKnownPosition;

    //Boss
    private GameObject _boss;
    [HideInInspector]public bool walkingToBoss = false;
    [Header("Boss")] 
    public float MaximumDistanceToBoss = 50.0f;

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
        BestCoverInReachNode bestCoverInReachNode = new BestCoverInReachNode(this);
        SequenceNode goToReachableCoverSequence = new SequenceNode(new List<Node> {bestCoverInReachNode, goToCoverNode});
        IsCoverReachableNode isCoverReachableNode = new IsCoverReachableNode(this);
        SequenceNode isCoverReachableSequence = new SequenceNode(new List<Node> {isCoverReachableNode, goToReachableCoverSequence});

        CurrentCoverValid currentCoverValidNode = new CurrentCoverValid(this);
        HidingFirstTimeNode hidingFirstTimeNode = new HidingFirstTimeNode(this);

        SelectorNode iDunno = new SelectorNode(new List<Node> {hidingFirstTimeNode, new InverterNode(currentCoverValidNode)});
        
        SequenceNode isHidingFirstTimeSequence = new SequenceNode(new List<Node> {iDunno, isCoverReachableSequence});
        
        //CurrentlyCovered "Sequence"
        CurrentlyCovered currentlyCoveredNode = new CurrentlyCovered(this);

        SelectorNode coverSelector = new SelectorNode(new List<Node> {currentlyCoveredNode, isHidingFirstTimeSequence, new InverterNode(repositionSequence)});

        IsValidCoverLeftNode isValidCoverLeftNode = new IsValidCoverLeftNode(this);
        
        CheckHealthNode checkHealthNode = new CheckHealthNode(this, criticalHealthThreshold);
        SequenceNode healthSequenceNode = new SequenceNode(new List<Node> {checkHealthNode, isValidCoverLeftNode, coverSelector});

        //Attack=======================================================================================================
        AttackNode attackNode = new AttackNode(this, _player.transform);
        PlayerInReachNode playerInReachNode = new PlayerInReachNode(this, _player.transform);
        PlayerInSight playerInSight = new PlayerInSight(this);
        SequenceNode playerInSightSequenceNode = new SequenceNode(new List<Node>() {playerInReachNode, playerInSight});
        SequenceNode attackSequenceNode = new SequenceNode(new List<Node> {playerInSightSequenceNode, attackNode});

        //Chase========================================================================================================
        ChaseNode chaseNode = new ChaseNode(this);
        PlayerCloseProximityNode playerCloseProximityNode = new PlayerCloseProximityNode(this);
        WasShotNode wasShotNode = new WasShotNode(this);
        SelectorNode playerIntelSelector = new SelectorNode(new List<Node>() {playerInSight, playerCloseProximityNode, wasShotNode});

        RotateToFindPlayerNode rotateToFindPlayerNode = new RotateToFindPlayerNode(this);

        SequenceNode chasePlayerSequence = new SequenceNode(new List<Node> {playerIntelSelector, chaseNode});

        SelectorNode chaseActionSelector =
            new SelectorNode(new List<Node>() {chasePlayerSequence, rotateToFindPlayerNode});
        
        //Patrol=======================================================================================================
        PatrolNode patrolNode = new PatrolNode(this);

        //Main=========================================================================================================
        //_mainNode = new SelectorNode(new List<Node> {healthSequenceNode, currentBossBehaviour, attackSequenceNode, chaseActionSelector, patrolNode});
        _mainNode = new SelectorNode(new List<Node> {healthSequenceNode, attackSequenceNode, chaseActionSelector, patrolNode});
    }

    
    //This is not even supposed to be here but in a node lol
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

    private void CheckForDeath()
    {
        if (CurrentHealth <= 0)
        {
            Destroy(NavAgent);
            gameObject.AddComponent<Rigidbody>();//.velocity = receivedBulletVelocity;
            Destroy(this);
        }
    }

    private void Update()
    {
        isCovered = false;
        _mainNode.EvaluateState();
        RegenerateHealth();
        ShowDebugInfo();
        CheckForDeath();
        WasShot = false;
    }

    private void GetAllHideables()
    {
        foreach (var hideableObject in GameObject.FindGameObjectsWithTag("Hideable"))
        {
            _hidePositions.Add(hideableObject);
        }
    }

    public List<GameObject> GetHideableObjects()
    {
        return _hidePositions;
    }

    public void RemoveHideableObject(GameObject objectToRemove)
    {
        _hidePositions.Remove(objectToRemove);
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
        _currentHealth = initialHealth;
        _hidePositions = new List<GameObject>();
        hidingFirstTime = true;
        NavAgent = GetComponent<NavMeshAgent>();
        NavAgent.isStopped = false;
    }

    private void ShowDebugInfo()
    {
        Debug.DrawRay(transform.position, transform.forward * shootRadius, Color.red);
        Debug.DrawRay(transform.position, Quaternion.Euler(0,-SightRangeAngle, 0) * (transform.forward * SightRadius), Color.magenta);
        Debug.DrawRay(transform.position, Quaternion.Euler(0,SightRangeAngle, 0) * (transform.forward * SightRadius), Color.magenta);
        Debug.DrawLine(transform.position, _boss.transform.position, Color.cyan);
        Debug.Log("Health is " + CurrentHealth);
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag.Equals("Bullet")) WasShot = true;
    }
}