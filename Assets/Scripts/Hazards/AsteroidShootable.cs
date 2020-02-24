using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidShootable : MonoBehaviour, IShootable
{
    public float myHealth = 100;
    public GameObject myModel;
    public AsteroidMove myAsteroidMove;
    public List<GameObject> explosionsPrefabList;

    private void Start()
    {
        if (myModel == null || myAsteroidMove == null || explosionsPrefabList.Count <= 0)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }
    }

    public bool IsAlive()
    {
        return (myHealth > 0);
    }

    public void DoDamage(float damageAmount)
    {
        myHealth -= damageAmount;
        if (myHealth <= 0)
        {
            myModel.SetActive(false);
            myAsteroidMove.enabled = false;
            GameObject explosionsPrefab = explosionsPrefabList[UnityEngine.Random.Range(0, explosionsPrefabList.Count)];
            GameObject explosionGameObject = Instantiate(explosionsPrefab, transform);
            explosionGameObject.transform.localPosition = Vector3.zero;
            explosionGameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            StartCoroutine(DestoryAsteroid(5f));
        }
    }

    private IEnumerator DestoryAsteroid(float timeout)
    {
        yield return new WaitForSeconds(timeout);
        Destroy(gameObject);
    }
}
