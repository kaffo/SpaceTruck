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
        Vector3 camTargetPos = new Vector3();
        Vector3 partsTargetPos = new Vector3();
        Vector3 partsStartPos = new Vector3();
        bool showGameObjects = false;

        switch (positionToSwitchTo)
        {
            case CAMERAPOSITION.PARTSHIP:
                camTargetPos = new Vector3(-3, 10, 0);
                partsStartPos = new Vector3(-7.5f, 0, -15);
                partsTargetPos = new Vector3(-7.5f, 0, 0);
                showGameObjects = true;
                break;
            default:
            case CAMERAPOSITION.DEFAULT:
                camTargetPos = new Vector3(0, 10, 0);
                partsStartPos = partsShip.transform.position;
                partsTargetPos = new Vector3(-7.5f, 0, 15);
                showGameObjects = false;
                ToggleHideGameObjects(showGameObjects); // Hide Early
                break;
        }

        partsShip.transform.position = partsStartPos;
        Vector3 camPos = mainGameCamera.transform.localPosition;
        while (Vector3.Distance(camPos, camTargetPos) > 0.01 || Vector3.Distance(partsShip.transform.position, partsTargetPos) > 0.01)
        {
            camPos = mainGameCamera.transform.localPosition = Vector3.MoveTowards(camPos, camTargetPos, 0.025f);

            float partsMoveSpeed = 0;
            if (positionToSwitchTo == CAMERAPOSITION.PARTSHIP)
            {
                partsMoveSpeed = Mathf.Clamp(Vector3.Distance(partsShip.transform.position, partsTargetPos) / 100, 0.015f, 0.075f);
            } else
            {
                partsMoveSpeed = Mathf.Clamp(Vector3.Distance(partsShip.transform.position, partsStartPos) / 100, 0.015f, 0.075f);
            }

            partsShip.transform.position = Vector3.MoveTowards(partsShip.transform.position, partsTargetPos, partsMoveSpeed);
            yield return new WaitForEndOfFrame();
        }

        // Show/Hide late
        if (positionToSwitchTo == CAMERAPOSITION.PARTSHIP)
        {
            ToggleHideGameObjects(showGameObjects);
        } else
        {
            partsShip.GetComponent<ShopInstance>().ClearShopSlots();
            partsShip.SetActive(false);

            moveScript.distToMove = 0.01f;
            gameCoroutine = StartCoroutine(StartTimedRun(timeOfRun));
            OnRunStart?.Invoke();
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
    }

    private IEnumerator StartTimedRun(float lengthOfRun)
    {
        yield return new WaitForSeconds(lengthOfRun);
        StopRun();
        // Fill parts ship
        partsShip.GetComponent<ShopInstance>().ClearShopSlots();
        partsShip.GetComponent<ShopInstance>().RandomFillShopSlots(1, 10);
        // Reward Player
        int reward = (int)(UnityEngine.Random.Range(100, 1000));
        MoneyManager.Instance.Reward(reward);
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
