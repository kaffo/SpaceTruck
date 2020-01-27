using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMove : MonoBehaviour {
    public float speedToMove = 0.05f;
    public float speedToRotate = 0.05f;

    private Vector3 moveTo;
    private int rotateDirX;
    private int rotateDirY;
    private int rotateDirZ;

    // Use this for initialization
    void Start () {
        int leftRight = (Random.value >= 0.5) ? 1 : -1;
        int upDown = (Random.value >= 0.5) ? 1 : -1; 
        Vector3 randomPlace = new Vector3(Random.Range(250, 1000) * leftRight, transform.position.y, Random.Range(250, 1000) * upDown);
        moveTo = (randomPlace - transform.position).normalized;
        rotateDirX = (Random.value >= 0.5) ? 1 : -1;
        rotateDirY = (Random.value >= 0.5) ? 1 : -1;
        rotateDirZ = (Random.value >= 0.5) ? 1 : -1;
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(speedToRotate * rotateDirX, speedToRotate * rotateDirY, speedToRotate * rotateDirZ);
        transform.position = transform.position + moveTo * speedToMove;
	}
}
