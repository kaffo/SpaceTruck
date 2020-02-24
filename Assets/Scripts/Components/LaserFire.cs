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

    [Header("References")]
    public GameObject myBarrel;

    private float lastFire = 0;
    private HashSet<GameObject> targetList = new HashSet<GameObject>();
    private GameObject currentTarget;
    private Collider myCollider;
    private Vector3 originalBarrelPosition;
    private Coroutine rotateBarrelRoutine;

    private void Start()
    {
        if (myAudioSource == null || laserFireSounds.Count <= 0 || myBarrel == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        myCollider = gameObject.GetComponent<Collider>();
        myCollider.enabled = false;

        originalBarrelPosition = myBarrel.transform.localPosition;

        GameManager.Instance.OnRunStart += OnRunStart;
        GameManager.Instance.OnRunEnd += OnRunEnd;
    }

    private void Awake()
    {
        currentTarget = null;
    }

    private void OnEnable()
    {
        rotateBarrelRoutine = StartCoroutine(RotateBarrelCoroutine());
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnRunStart -= OnRunStart;
            GameManager.Instance.OnRunEnd -= OnRunEnd;
        }

        if (rotateBarrelRoutine != null)
            StopCoroutine(rotateBarrelRoutine);
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

    private IEnumerator RotateBarrelCoroutine()
    {
        while (true)
        {
            if (currentTarget != null)
            {
                RotateTurret(currentTarget.transform);
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private void RotateTurret(Transform target)
    {
        Vector3 targetVector = (target.position - transform.position).normalized;
        targetVector.y = 0;
        float angleToTarget = Vector3.SignedAngle(myBarrel.transform.forward, targetVector, Vector3.up);
        if (Mathf.Abs(angleToTarget) > 1f)
        {
            myBarrel.transform.RotateAround(transform.position, Vector3.up, angleToTarget);
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
            // If the target has been destoried, remove it
            if (target == null) 
            {
                Debug.Log($"Removed null target");
                targetList.Remove(target);
                if (target == currentTarget) { currentTarget = null; }
                continue;
            }

            // If the target is dead, but hasn't been destoried yet, remove it
            if (target != null)
            {
                IShootable shootableTarget = target.GetComponent(typeof(IShootable)) as IShootable;
                if (!shootableTarget.IsAlive())
                {
                    Debug.Log($"Removed {target.name} as dead");
                    targetList.Remove(target);
                    if (target == currentTarget) { currentTarget = null; }
                    continue;
                }
            }

            if (target != null && currentTarget == null)
            {
                Debug.Log($"Current target now {target.name}");
                currentTarget = target;
            }

            // If the target is there, and we can fire, then fire
            if (target != null && (Time.time - rateOfFire) > lastFire)
            {
                IShootable shootableTarget = target.GetComponent(typeof(IShootable)) as IShootable;
                RotateTurret(target.transform);

                Debug.Log($"{gameObject.name} shot {target.name} for {damage} damage");
                myAudioSource.PlayOneShot(laserFireSounds[UnityEngine.Random.Range(0, laserFireSounds.Count)]);

                // If we kill the target, remove it
                if (shootableTarget.DoDamage(damage)) 
                { 
                    targetList.Remove(target);
                    currentTarget = null;
                }

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
