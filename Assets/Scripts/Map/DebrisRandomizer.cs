using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisRandomizer : MonoBehaviour {
    public GameObject asteroidPrefab;
    public int maxAsteroids = 100;
    public int minAsteroids = 15;
    public Vector3 spawnAreaSize = new Vector3(100f, 0f, 100f);
    public Vector3 exclusionZone = new Vector3(0f, 0f, 0f);

	// Use this for initialization
	void Start () {
        if (asteroidPrefab == null)
        {
            Debug.LogError("No prefab selected");
            this.enabled = false;
            return;
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
            GameObject asteroid = Instantiate(asteroidPrefab, this.transform, false);
            asteroid.transform.localPosition = new Vector3(x, y, z);
            asteroid.transform.rotation = Random.rotation;
        }
    }
}
