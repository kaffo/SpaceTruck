using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidRandomizer : MonoBehaviour {
    public GameObject myModel;
    [Header("Settings")]
    public float biggestScale = 10f;
    public float smallestScale = 1f;

    // Use this for initialization
    void Start () {
        if (myModel == null)
        {
            Debug.LogError(this.name + "can't find model!");
            this.enabled = false;
            return;
        }

        myModel.transform.localScale =  new Vector3(Random.Range(smallestScale, biggestScale), Random.Range(smallestScale, biggestScale), Random.Range(smallestScale, biggestScale));
	}
}
