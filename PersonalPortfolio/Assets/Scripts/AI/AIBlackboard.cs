using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBlackboard : MonoBehaviour
{
    [SerializeField]
    private float initialHealth = 100.0f;
    private float currentHealth;

    [SerializeField]
    private float criticalHealthThreshold = 20.0f;

    public float CurrentHealth => currentHealth; 


    SelectorNode MainNode = null;

    void Start()
    {
        currentHealth = initialHealth;
        CreateBehaviorTree();
    }

    private void CreateBehaviorTree()
    {
        CheckHealthNode checkHealthNode = new CheckHealthNode(this, criticalHealthThreshold);
        
        SequenceNode Health = new SequenceNode(new List<Node> { checkHealthNode });
        
        MainNode = new SelectorNode(new List<Node> { Health });
    }

    private void Update()
    {
        if(MainNode.EvaluateState() == State.Failure) Debug.Log("Health was over the threshold!");
    }
}
