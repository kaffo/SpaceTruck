using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ArmourHitCheck : MonoBehaviour
{
    public bool beenHit = false;
    private void OnTriggerEnter(Collider other)
    {
        ShipDamage shipDamageScript = other.GetComponent<ShipDamage>();
        if (shipDamageScript == null)
            shipDamageScript = other.transform.parent.GetComponent<ShipDamage>();

        if (shipDamageScript != null)
        {
            if (!beenHit)
            {
                Debug.Log($"Hit on {gameObject.name}!");
                beenHit = true;
            } else
            {
                GameManager.Instance.GameOver();
            }
        }
    }
}
