using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActiveOnGrab : MonoBehaviour
{
    public GameObject toggleGameObject;
    public bool startOn = false;

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
        EventManager.Instance.OnPlayerPickupComponent += OnPlayerGrabComponent;
        EventManager.Instance.OnPlayerDropComponent += OnPlayerDropComponent;
    }

    private void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnPlayerPickupComponent -= OnPlayerGrabComponent;
            EventManager.Instance.OnPlayerDropComponent -= OnPlayerDropComponent;
        }
    }

    private bool IsMyParent()
    {
        Transform parentTransform = EventManager.Instance.componentInMouse.myItemSlot.transform;
        bool myParent = false;
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            if (parentTransform.GetChild(i).gameObject == gameObject)
            {
                myParent = true;
                break;
            }
        }
        return myParent;
    }

    private void OnPlayerGrabComponent()
    {

        if (IsMyParent()) { toggleGameObject.SetActive(!startOn); }
    }

    private void OnPlayerDropComponent()
    {
        toggleGameObject.SetActive(startOn);
    }
}
