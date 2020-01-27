using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Settings")]
    public float timeOfRun = 10f;

    [Header("References")]
    public Camera mainGameCamera;
    public GameObject partsShip;
    public ShipMovement moveScript;
    public List<GameObject> gameObjectsToHide;

    public delegate void OnRunStartDelegate();
    public event OnRunStartDelegate OnRunStart;

    public delegate void OnRunEndDelegate();
    public event OnRunEndDelegate OnRunEnd;

    private enum CAMERAPOSITION
    {
        DEFAULT,
        PARTSHIP
    }

    private Coroutine gameCoroutine;

    private void Start()
    {
        if (mainGameCamera == null || partsShip == null || moveScript == null || gameObjectsToHide == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        ShopInstance partsShipShop = partsShip.GetComponent<ShopInstance>();
        if (partsShipShop != null)
        {
            List<Tuple<int, Definitions.SHIPCOMPONENTS>> startingComponents = new List<Tuple<int, Definitions.SHIPCOMPONENTS>>
            {
                new Tuple<int, Definitions.SHIPCOMPONENTS>(0, Definitions.SHIPCOMPONENTS.LASER),
                new Tuple<int, Definitions.SHIPCOMPONENTS>(0, Definitions.SHIPCOMPONENTS.LASER),
                new Tuple<int, Definitions.SHIPCOMPONENTS>(0, Definitions.SHIPCOMPONENTS.LASER)
            };
            partsShipShop.SetShopSlots(startingComponents);
        }

        // Move the camera, we'll fix this later
        StartCoroutine(LerpCameraToPosition(CAMERAPOSITION.PARTSHIP));
    }

    private IEnumerator LerpCameraToPosition(CAMERAPOSITION positionToSwitchTo)
    {
        Vector3 targetPos = new Vector3();
        bool showGameObjects = false;

        switch (positionToSwitchTo)
        {
            case CAMERAPOSITION.PARTSHIP:
                targetPos = new Vector3(-3, 10, 0);
                showGameObjects = true;
                break;
            default:
            case CAMERAPOSITION.DEFAULT:
                targetPos = new Vector3(0, 10, 0);
                showGameObjects = false;
                ToggleHideGameObjects(showGameObjects); // Hide Early
                break;
        }
        
        Vector3 camPos = mainGameCamera.transform.localPosition;
        while (Vector3.Distance(camPos, targetPos) > 0.01)
        {
            camPos = mainGameCamera.transform.localPosition = Vector3.MoveTowards(camPos, targetPos, 0.025f);
            yield return new WaitForEndOfFrame();
        }

        // Show/Hide late
        if (positionToSwitchTo == CAMERAPOSITION.PARTSHIP)
        {
            ToggleHideGameObjects(showGameObjects);
        }
    }

    private void ToggleHideGameObjects(bool show)
    {
        if (gameObjectsToHide.Count > 0)
        {
            foreach (GameObject gameObjectToHide in gameObjectsToHide)
            {
                gameObjectToHide.SetActive(show);
            }
        }
    }

    public void StartRun()
    {
        Debug.Log("Starting Run");
        StartCoroutine(LerpCameraToPosition(CAMERAPOSITION.DEFAULT)); // TODO move this
        partsShip.SetActive(false);

        moveScript.distToMove = 0.01f;
        gameCoroutine = StartCoroutine(StartTimedRun(timeOfRun));
        OnRunStart?.Invoke();
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

        StartCoroutine(LerpCameraToPosition(CAMERAPOSITION.PARTSHIP)); // TODO move this
        OnRunEnd?.Invoke();
    }

    public void GameOver()
    {
        StopCoroutine(gameCoroutine);
        Debug.Log("Game Over - Ending Run");
        moveScript.distToMove = 0.00f;
        OnRunEnd?.Invoke();
    }
}
