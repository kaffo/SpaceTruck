using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LaserFire : MonoBehaviour
{
    [Header("Settings")]
    public float rateOfFire = 5f;
    public float damage = 100f;

    [Header("Audio")]
    public AudioSource myAudioSource;
    public List<AudioClip> laserFireSounds;

    private float lastFire = 0;
    private HashSet<GameObject> targetList = new HashSet<GameObject>();
    private Collider myCollider;

    private void Start()
    {
        if (myAudioSource == null || laserFireSounds.Count <= 0)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        myCollider = gameObject.GetComponent<Collider>();
        myCollider.enabled = false;

        GameManager.Instance.OnRunStart += OnRunStart;
        GameManager.Instance.OnRunEnd += OnRunEnd;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnRunStart -= OnRunStart;
            GameManager.Instance.OnRunEnd -= OnRunEnd;
        }
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
                if (!shootableTarget.IsAlive()) { continue; }

                Debug.Log($"{gameObject.name} shot {target.name} for {damage} damage");
                myAudioSource.PlayOneShot(laserFireSounds[UnityEngine.Random.Range(0, laserFireSounds.Count)]);
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
