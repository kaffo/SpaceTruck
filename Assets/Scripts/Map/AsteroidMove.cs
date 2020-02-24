using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMove : MonoBehaviour {
    
    public float speedToMove = 0.05f;
    
    [Header("Random Speed")]
    public bool randomSpeed = false;
    public float randomSpeedDelta = 0.01f;

    public float speedToRotate = 0.05f;
    public Vector3 targetPosition = new Vector3(0, 0, 0);

    private int rotateDirX;
    private int rotateDirY;
    private int rotateDirZ;

    // Use this for initialization
    void Start () {
        rotateDirX = (Random.value >= 0.5) ? 1 : -1;
        rotateDirY = (Random.value >= 0.5) ? 1 : -1;
        rotateDirZ = (Random.value >= 0.5) ? 1 : -1;
        
        if (randomSpeed) { speedToMove = speedToMove + UnityEngine.Random.Range(0, randomSpeedDelta); }
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(speedToRotate * rotateDirX, speedToRotate * rotateDirY, speedToRotate * rotateDirZ);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speedToMove);
	}
}
