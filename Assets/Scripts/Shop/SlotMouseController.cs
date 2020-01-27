using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(PriceText))]
public class SlotMouseController : MonoBehaviour
{
    [Header("Settings")]
    public Definitions.SHIPCOMPONENTS myShipComponent;

    [Header("References")]
    public Material defaultMaterial;
    public Material onHoverMaterial;
    public GameObject myBaseModel;

    private Renderer myBaseRenderer;
    private bool grabbed = false;

    private void Start()
    {
        if (defaultMaterial == null || onHoverMaterial == null || myBaseModel == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        myBaseRenderer = myBaseModel.GetComponent<Renderer>();
        if (myBaseRenderer == null)
        {
            Debug.LogError($"Couldn't get renderer from {gameObject.name}");
            this.enabled = false;
            return;
        }

        myBaseRenderer.material = defaultMaterial;
    }

    private void OnMouseEnter()
    {
        myBaseRenderer.material = onHoverMaterial;
    }

    private void OnMouseExit()
    {
        myBaseRenderer.material = defaultMaterial;
    }

    private void OnMouseDown()
    {
        if (EventManager.Instance.PlayerHasComponent)
        {
            Debug.LogWarning("Cannot hold two components at once");
            return;
        }

        EventManager.Instance.PlayerHasComponent = true;
        EventManager.Instance.componentInMouse = this;
        grabbed = true;
    }

    private void OnMouseUp()
    {
        if (!EventManager.Instance.PlayerHasComponent || !grabbed)
        {
            return;
        }

        EventManager.Instance.PlayerHasComponent = false;
        EventManager.Instance.componentInMouse = null;
        grabbed = false;

        if (EventManager.Instance.attachpointUnderMouse)
        {
            // TODO money

            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponent<PriceText>().ItemPriceString = "SOLD";
            myBaseModel.SetActive(false);
            EventManager.Instance.attachpointUnderMouse.InstallComponent(myShipComponent);
        }
    }
}
