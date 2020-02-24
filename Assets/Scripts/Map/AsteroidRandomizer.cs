using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidRandomizer : MonoBehaviour {
    public GameObject myModel;
    [Header("Settings")]
    public bool changeScale = true;
    public float biggestScale = 10f;
    public float smallestScale = 1f;

    [Header("Model Choices")]
    public List<Mesh> asteroidMeshes;

    // Use this for initialization
    void Start () {
        if (myModel == null)
        {
            Debug.LogError(this.name + "can't find model!");
            this.enabled = false;
            return;
        }

        if (changeScale)
            myModel.transform.localScale =  new Vector3(Random.Range(smallestScale, biggestScale), Random.Range(smallestScale, biggestScale), Random.Range(smallestScale, biggestScale));

        if (asteroidMeshes.Count > 0)
        {
            Mesh myMesh = asteroidMeshes[UnityEngine.Random.Range(0, asteroidMeshes.Count)];
            myModel.GetComponent<MeshFilter>().mesh = myMesh;
        }
	}
}
