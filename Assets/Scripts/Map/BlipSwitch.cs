using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlipSwitch : MonoBehaviour {
    public bool isVisible = false;
    public GameObject visibleModel;
    public GameObject blipObject;

    private void Start()
    {
        if (visibleModel == null || blipObject == null)
        {
            Debug.LogError("Objects not set on " + this.name);
            this.enabled = false;
            return;
        }
    }

    // Update is called once per frame
    void Update () {
		if (isVisible)
        {
            visibleModel.SetActive(true);
            blipObject.SetActive(false);
        } else
        {
            visibleModel.SetActive(false);
            blipObject.SetActive(true);
        }
	}
}
