using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LaserFire : MonoBehaviour
{
    public float rateOfFire = 5f;
    public float damage = 100f;

    private float lastFire = 0;
    private HashSet<GameObject> targetList = new HashSet<GameObject>();
    private Collider myCollider;

    private void Start()
    {
        myCollider = gameObject.GetComponent<Collider>();
        myCollider.enabled = false;

        GameManager.Instance.OnRunStart += OnRunStart;
        GameManager.Instance.OnRunEnd += OnRunEnd;
    }

    private void OnTriggerEnter(Collider other)
    {
        IShootable otherShootable = other.GetComponent(typeof(IShootable)) as IShootable;
        GameObject otherGameobject = other.gameObject;
        if (otherShootable == null)
        {
            otherShootable = other.transform.parent.GetComponent(typeof(IShootable)) as IShootable;
            otherGameobject = other.transform.parent.gameObject;
        }

        if (otherShootable != null && !targetList.Contains(otherGameobject))
        {
            targetList.Add(otherGameobject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IShootable otherShootable = other.GetComponent(typeof(IShootable)) as IShootable;
        GameObject otherGameobject = other.gameObject;
        if (otherShootable == null)
        {
            otherShootable = other.transform.parent.GetComponent(typeof(IShootable)) as IShootable;
            otherGameobject = other.transform.parent.gameObject;
        }

        if (otherShootable != null && targetList.Contains(otherGameobject))
        {
            targetList.Remove(otherGameobject);
        }
    }

    private void Update()
    {
        if (targetList.Count <= 0)
        {
            return;
        }

        foreach (GameObject target in new List<GameObject>(targetList))
        {
            if (target == null) { targetList.Remove(target); }
            if (target != null && (Time.time - rateOfFire) > lastFire)
            {
                IShootable shootableTarget = target.GetComponent(typeof(IShootable)) as IShootable;
                Debug.Log($"{gameObject.name} shot for {damage} damage");
                shootableTarget.DoDamage(damage);
                lastFire = Time.time;
            }
        }
    }

    private void OnRunStart()
    {
        myCollider.enabled = true;
    }

    private void OnRunEnd()
    {
        myCollider.enabled = false;
    }
}
