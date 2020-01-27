using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("References")]
    public GameObject partsShip;
    public ShipMovement moveScript;
    public List<GameObject> gameObjectsToHide;

    private void Start()
    {
        if (partsShip == null || moveScript == null || gameObjectsToHide == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }
    }

    public void StartRun()
    {
        Debug.Log("Starting Run");
        partsShip.SetActive(false);

        if (gameObjectsToHide.Count > 0)
        {
            foreach (GameObject gameObjectToHide in gameObjectsToHide)
            {
                gameObjectToHide.SetActive(false);
            }
        }

        moveScript.distToMove = 0.01f;
        StartCoroutine(StartTimedRun(5f));
    }

    private IEnumerator StartTimedRun(float lengthOfRun)
    {
        yield return new WaitForSeconds(lengthOfRun);
        StopRun();
    }

    public void StopRun()
    {
        Debug.Log("Ending Run");
        moveScript.distToMove = 0.00f;
        partsShip.SetActive(true);

        if (gameObjectsToHide.Count > 0)
        {
            foreach (GameObject gameObjectToHide in gameObjectsToHide)
            {
                gameObjectToHide.SetActive(true);
            }
        }
    }
}
