using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthIndicatorUI : MonoBehaviour
{
    [SerializeField] private GameObject greenHealthObject;
    [SerializeField] private GameObject redHealthObject;

    private bool beenHit = false;
    public bool BeenHit
    {
        set
        {
            beenHit = value;
            greenHealthObject.SetActive(!beenHit);
            redHealthObject.SetActive(beenHit);
        }
        get { return beenHit; }
    }

    private void Start()
    {
        if (greenHealthObject == null || redHealthObject == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }
    }
}
