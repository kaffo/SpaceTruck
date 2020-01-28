using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideOnComponentGrab : MonoBehaviour
{
    public GameObject objectToToggle;
    public bool enableOnGrab = true;
    public bool enableOnDrop = true;

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
        EventManager.Instance.OnPlayerPickupComponent += OnPickup;
        EventManager.Instance.OnPlayerDropComponent += OnDrop;
    }

    private void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnPlayerPickupComponent -= OnPickup;
            EventManager.Instance.OnPlayerDropComponent -= OnDrop;
        }
    }

    private void OnPickup()
    {
        objectToToggle.SetActive(enableOnGrab);
    }

    private void OnDrop()
    {
        objectToToggle.SetActive(enableOnDrop);
    }
}
