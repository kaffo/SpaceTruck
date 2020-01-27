using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPointEventHandler : MonoBehaviour
{
    [Header("References")]
    public Material defaultMaterial;
    public Material onHoverMaterial;
    public GameObject myHoverModel;

    private Renderer myHoverRenderer;
    private Collider myCollider;

    private void Start()
    {
        if (defaultMaterial == null || onHoverMaterial == null || myHoverModel == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        myHoverRenderer = myHoverModel.GetComponent<Renderer>();
        if (myHoverRenderer == null)
        {
            Debug.LogError($"Couldn't get renderer from {gameObject.name}");
            this.enabled = false;
            return;
        }

        myHoverRenderer.material = defaultMaterial;
        myHoverModel.SetActive(false);

        myCollider = gameObject.GetComponent<Collider>();
        if (myCollider == null)
        {
            Debug.LogError($"Couldn't get collider from {gameObject.name}");
            this.enabled = false;
            return;
        }

        myCollider.enabled = false;
        EventManager.Instance.OnPlayerPickupComponent += OnPlayerGrabComponent;
        EventManager.Instance.OnPlayerDropComponent += OnPlayerDropComponent;
    }

    private void OnPlayerGrabComponent()
    {
        myHoverRenderer.material = defaultMaterial;
        myHoverModel.SetActive(true);
        myCollider.enabled = true;
    }

    private void OnPlayerDropComponent()
    {
        myHoverModel.SetActive(false);
        myCollider.enabled = false;
    }

    void OnMouseEnter()
    {
        myHoverRenderer.material = onHoverMaterial;
    }

    private void OnMouseExit()
    {
        myHoverRenderer.material = defaultMaterial;
    }
}
