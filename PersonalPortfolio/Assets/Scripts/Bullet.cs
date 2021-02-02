using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float BulletDamage = 10.0f;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            AIBlackboard aiBlackboard = other.gameObject.GetComponent<AIBlackboard>();
            if(aiBlackboard != null) aiBlackboard.ReduceHealth(BulletDamage);
        }
    }
}
