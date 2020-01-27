using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideOnRun : MonoBehaviour
{
    public GameObject objectToToggle;
    public bool enableOnRunStart = true;
    public bool enableOnRunEnd = true;

    private void Start()
    {
        if (objectToToggle == null)
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
        objectToToggle.SetActive(enableOnRunStart);
    }

    private void OnRunEnd()
    {
        objectToToggle.SetActive(enableOnRunEnd);
    }
}
