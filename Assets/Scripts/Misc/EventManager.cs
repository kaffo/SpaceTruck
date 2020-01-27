using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    private bool playerHasComponent = false;
    public bool PlayerHasComponent
    {
        get { return playerHasComponent; }
        set {
            if (playerHasComponent == value)
            {
                Debug.LogWarning($"Possible logic mistake as {value} is trying to be set to playerHasComponent when already set");
                return;
            }

            if (value && OnPlayerPickupComponent != null)
            {
                OnPlayerPickupComponent();
            } else if (!value && OnPlayerDropComponent != null)
            {
                OnPlayerDropComponent();
            }

            playerHasComponent = value;
        }
    }

    public delegate void OnPlayerPickupComponentDelegate();
    public event OnPlayerPickupComponentDelegate OnPlayerPickupComponent;

    public delegate void OnPlayerDropComponentDelegate();
    public event OnPlayerDropComponentDelegate OnPlayerDropComponent;
}
