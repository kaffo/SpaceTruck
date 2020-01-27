using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidShootable : MonoBehaviour, IShootable
{
    public float myHealth = 100;

    public void DoDamage(float damageAmount)
    {
        myHealth -= damageAmount;
        if (myHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
