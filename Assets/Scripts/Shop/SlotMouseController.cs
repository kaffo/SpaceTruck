using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(PriceText))]
public class SlotMouseController : MonoBehaviour
{
    [Header("Settings")]
    public Definitions.SHIPCOMPONENTS myShipComponent;
    public int myPrice = 0;

    [Header("References")]
    public Material defaultMaterial;
    public Material onHoverMaterial;
    public Material onHoverProblemMaterial;
    public GameObject myBaseModel;
    public GameObject myItemSlot;

    private Renderer myBaseRenderer;
    private PriceText myPriceText;
    private bool grabbed = false;

    private void Start()
    {
        if (defaultMaterial == null || onHoverMaterial == null || onHoverProblemMaterial == null || myBaseModel == null || myItemSlot == null)
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

        myPriceText = gameObject.GetComponent<PriceText>();
        SetPrice();
    }

    private void SetPrice()
    {
        if (myPriceText != null)
        {
            if (myPrice == 0)
            {
                myPriceText.ItemPriceString = "FREE";
            }
            else
            {
                myPriceText.ItemPriceString = $"${myPrice}";
            }
        }
    }

    private void OnMouseEnter()
    {
        if (MoneyManager.Instance.CanAfford(myPrice))
        {
            myBaseRenderer.material = onHoverMaterial;
        } else
        {
            myBaseRenderer.material = onHoverProblemMaterial;
        }
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

        if (!MoneyManager.Instance.CanAfford(myPrice))
        {
            Debug.LogWarning($"Can't afford {myPrice}");
            return;
        }

        EventManager.Instance.PlayerHasComponent = true;
        EventManager.Instance.componentInMouse = this;
        grabbed = true;
    }

    private void DeleteSlotItems()
    {
        for (int i = 0; i < myItemSlot.transform.childCount; i++)
        {
            Destroy(myItemSlot.transform.GetChild(i).gameObject);
        }
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
            MoneyManager.Instance.Purchase(myPrice);

            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponent<PriceText>().ItemPriceString = "SOLD";
            myBaseModel.SetActive(false);
            DeleteSlotItems();
            EventManager.Instance.attachpointUnderMouse.InstallComponent(myShipComponent);
        }
    }
}
