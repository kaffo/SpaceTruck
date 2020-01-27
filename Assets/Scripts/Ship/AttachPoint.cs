using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPoint : MonoBehaviour
{
    public GameObject myAttachPoint;
    public GameObject myModel;
    public bool HasComponentAttached
    {
        get {
            bool hasAttached = myAttachPoint.transform.childCount > 0;

            // Show/Hide the base plate depending on if we have an attachment or not
            myModel.SetActive(!hasAttached);
            return hasAttached; }
    }

    private void Start()
    {
        if (myAttachPoint == null || myModel == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }
    }

    public void AttachComponent(GameObject component)
    {
        if (HasComponentAttached)
        {
            Debug.LogWarning($"Cannot attach {component.name} as {myAttachPoint.transform.GetChild(0).name} is already attached");
            return;
        }

        component.transform.SetParent(myAttachPoint.transform);
        component.transform.localPosition = Vector3.zero;
        component.transform.localRotation = Quaternion.identity;

        if (!HasComponentAttached)
        {
            Debug.LogError($"Error attaching {component.name}!");
            return;
        }
    }
}
