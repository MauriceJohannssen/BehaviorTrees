using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float BulletDamage = 30.0f;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            other.gameObject.GetComponent<AIBlackboard>().ReduceHealth(BulletDamage);
        }
    }
}
