﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Settings")]
    public float timeOfRun = 10f;
    public int totalRuns = 5;
    public float successfulRunReward = 500f;
    public float shipLowPitch = 0.5f;
    public float shipHighPitch = 1.5f;

    [Header("References")]
    public Camera mainGameCamera;
    public GameObject partsShip;
    public ShipMovement moveScript;
    public AudioSource shipAudio;
    public List<GameObject> gameObjectsToHide;
    public GameOverRunsCounter gameOverUIScript;
    public List<AsteroidManager> asteroidManagers;

    [HideInInspector]
    public int runCount = 1;

    public delegate void OnRunStartDelegate();
    public event OnRunStartDelegate OnRunStart;

    public delegate void OnRunEndDelegate();
    public event OnRunEndDelegate OnRunEnd;

    public delegate void OnThreatChangeDelegate(THREAT_LEVEL top, THREAT_LEVEL right, THREAT_LEVEL bottom, THREAT_LEVEL left);
    public event OnThreatChangeDelegate OnThreatChange;

    private enum CAMERAPOSITION
    {
        DEFAULT,
        PARTSHIP
    }

    public enum THREAT_LEVEL
    {
        NONE,
        LOW,
        MEDIUM,
        HIGH
    }

    private Coroutine gameCoroutine;

    private void Start()
    {
        if (mainGameCamera == null || partsShip == null || moveScript == null || shipAudio == null ||
            gameObjectsToHide == null || gameOverUIScript == null || asteroidManagers.Count != 4)
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
                new Tuple<int, Definitions.SHIPCOMPONENTS>(500, Definitions.SHIPCOMPONENTS.LASER),
                new Tuple<int, Definitions.SHIPCOMPONENTS>(500, Definitions.SHIPCOMPONENTS.LASER),
                new Tuple<int, Definitions.SHIPCOMPONENTS>(500, Definitions.SHIPCOMPONENTS.CARGO_TRACTOR)
            };
            partsShipShop.SetShopSlots(startingComponents);
        }

        // Move the camera, we'll fix this later
        StartCoroutine(LerpCameraToPosition(CAMERAPOSITION.PARTSHIP));
        OnThreatChange?.Invoke(GetThreatLevel(asteroidManagers[0].maxAsteroids * 20),
            GetThreatLevel(asteroidManagers[1].maxAsteroids * 20),
            GetThreatLevel(asteroidManagers[3].maxAsteroids * 20),
            GetThreatLevel(asteroidManagers[2].maxAsteroids * 20));
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
            StartCoroutine(IncreaseShipPitchSound());
            OnRunStart?.Invoke();
        }
    }

    private IEnumerator IncreaseShipPitchSound()
    {
        while (shipAudio.pitch < shipHighPitch)
        {
            shipAudio.pitch += 0.01f;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator DecreaseShipPitchSound()
    {
        while (shipAudio.pitch > shipLowPitch)
        {
            shipAudio.pitch -= 0.01f;
            yield return new WaitForEndOfFrame();
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

    //0 - 100
    private THREAT_LEVEL GetThreatLevel(int threatInt)
    {
        if (threatInt >= 50)
        {
            return THREAT_LEVEL.HIGH;
        }
        else if (threatInt >= 25)
        {
            return THREAT_LEVEL.MEDIUM;
        }
        else if (threatInt >= 0)
        {
            return THREAT_LEVEL.LOW;
        }
        else
        {
            return THREAT_LEVEL.NONE;
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

        // Reward Player
        int reward = (int)successfulRunReward;
        MoneyManager.Instance.Reward(reward);

        // Check end of game
        if (++runCount < totalRuns) 
        {
            StopRun();
            // Fill parts ship
            partsShip.GetComponent<ShopInstance>().ClearShopSlots();
            partsShip.GetComponent<ShopInstance>().RandomFillShopSlots(1, 10);
        } else
        {
            GameOver();
        }
    }

    private void IncreaseDifficulty()
    {
        float randomNumber = UnityEngine.Random.value;
        if (randomNumber < 0.4)
        {
            asteroidManagers[0].maxAsteroids++;
        } else if (randomNumber < 0.65)
        {
            asteroidManagers[1].maxAsteroids++;
        }else if (randomNumber < 0.9)
        {
            asteroidManagers[2].maxAsteroids++;
        } else
        {
            asteroidManagers[3].maxAsteroids++;
        }
        OnThreatChange?.Invoke(GetThreatLevel(asteroidManagers[0].maxAsteroids * 20),
            GetThreatLevel(asteroidManagers[1].maxAsteroids * 20),
            GetThreatLevel(asteroidManagers[3].maxAsteroids * 20),
            GetThreatLevel(asteroidManagers[2].maxAsteroids * 20));
    }

    public void StopRun()
    {
        Debug.Log("Ending Run");
        moveScript.distToMove = 0.00f;

        partsShip.SetActive(true);
        StartCoroutine(LerpCameraToPosition(CAMERAPOSITION.PARTSHIP)); // TODO move this
        StartCoroutine(DecreaseShipPitchSound());
        IncreaseDifficulty();
        OnRunEnd?.Invoke();
    }

    public void GameOver()
    {
        StopCoroutine(gameCoroutine);
        shipAudio.enabled = false;
        Debug.Log("Game Over - Ending Run");
        moveScript.distToMove = 0.00f;
        OnRunEnd?.Invoke();
        gameOverUIScript.SetGameOverText();
        gameOverUIScript.gameObject.SetActive(true);
    }
}
