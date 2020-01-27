using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour {
    public enum Direction { North, East, South, West, Up, Down};
    [Header("Game Object")]
    public GameObject toMove;
    [Header("Settings")]
    public Direction moveDirection = Direction.North;
    public float distToMove = 0.1f;

    // Use this for initialization
    void Start () {
        if (toMove == null)
        {
            Debug.LogError("No Object Set!");
            this.enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        switch (moveDirection)
        {
            case Direction.North:
                toMove.transform.Translate(0f, 0f, distToMove);
                break;
            case Direction.East:
                toMove.transform.Translate(distToMove, 0f, 0f);
                break;
            case Direction.South:
                toMove.transform.Translate(0f, 0f, -distToMove);
                break;
            case Direction.West:
                toMove.transform.Translate(-distToMove, 0f, 0f);
                break;
            case Direction.Up:
                toMove.transform.Translate(0f, distToMove, 0f);
                break;
            case Direction.Down:
                toMove.transform.Translate(0f, -distToMove, 0f);
                break;
            default:
                Debug.LogError("Cannot find direction");
                this.enabled = false;
                break;
        }
	}
}
