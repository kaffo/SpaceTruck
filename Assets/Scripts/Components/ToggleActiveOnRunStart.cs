using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActiveOnRunStart : MonoBehaviour
{
    public GameObject toggleGameObject;
    public bool enableOnRun = true;

    private void Start()
    {
        if (toggleGameObject == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.OnRunStart += OnRunStart;
        GameManager.Instance.OnRunEnd += OnRunEnd;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnRunStart -= OnRunStart;
            GameManager.Instance.OnRunEnd -= OnRunEnd;
        }
    }

    private void OnRunStart()
    {

        toggleGameObject.SetActive(enableOnRun);
    }

    private void OnRunEnd()
    {
        toggleGameObject.SetActive(!enableOnRun);
    }
}
