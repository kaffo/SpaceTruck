using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    public GameObject frontTargets;
    public GameObject asteroidPrefab;
    public int maxAsteroids = 100;
    public int minAsteroids = 15;
    public Vector3 spawnOrigin = new Vector3(0f, 0f, 0f);
    public Vector3 spawnAreaSize = new Vector3(100f, 0f, 100f);
    public Vector3 exclusionZone = new Vector3(0f, 0f, 0f);

    private void Start()
    {
        if (frontTargets == null || asteroidPrefab == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        GameManager.Instance.OnRunStart += OnRunStart;
        GameManager.Instance.OnRunEnd += OnRunEnd;
    }

    private void OnRunStart()
    {
        List<Vector3> targetVectorList = new List<Vector3>();

        foreach (Transform target in frontTargets.GetComponentsInChildren<Transform>())
        {
            if (target == frontTargets.transform) { continue; }
            targetVectorList.Add(target.position);
        }

        int numToSpawn = Random.Range(minAsteroids, maxAsteroids);

        for (int i = 0; i < numToSpawn; i++)
        {
            float x = Random.Range(exclusionZone.x, spawnAreaSize.x);
            float y = Random.Range(exclusionZone.y, spawnAreaSize.y);
            float z = Random.Range(exclusionZone.z, spawnAreaSize.z);
            if (Random.value >= 0.5) { x = -x; }
            if (Random.value >= 0.5) { y = -y; }
            if (Random.value >= 0.5) { z = -z; }
            x += spawnOrigin.x;
            y += spawnOrigin.y;
            z += spawnOrigin.z;
            GameObject asteroid = Instantiate(asteroidPrefab, this.transform, false);
            asteroid.transform.localPosition = new Vector3(x, y, z);
            asteroid.transform.rotation = Random.rotation;
            AsteroidMove asteroidMoveScript = asteroid.GetComponent<AsteroidMove>();

            if (!asteroidMoveScript)
            {
                Debug.LogWarning($"Couldn't find move script on {asteroid.name}");
                return;
            }

            asteroidMoveScript.targetPosition = targetVectorList[(int)(Random.value * targetVectorList.Count)];
            asteroidMoveScript.enabled = true;
        }
    }

    private void OnRunEnd()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform asteroid = transform.GetChild(i);
            Destroy(asteroid.gameObject);
        }
    }
}
