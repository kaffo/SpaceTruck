using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttachPoint))]
public class AttachPointEventHandler : MonoBehaviour
{
    [Header("References")]
    public Material defaultMaterial;
    public Material onHoverMaterial;
    public GameObject myHoverModel;
    public GameObject myBaseModel;
    public GameObject laserPrefab;
    public ShowHideOnRun baseHideScript;

    private Renderer myHoverRenderer;
    private Collider myCollider;

    private void Start()
    {
        if (defaultMaterial == null || onHoverMaterial == null || myHoverModel == null || laserPrefab == null || baseHideScript == null)
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
        EventManager.Instance.attachpointUnderMouse = this;
    }

    private void OnMouseExit()
    {
        myHoverRenderer.material = defaultMaterial;
        EventManager.Instance.attachpointUnderMouse = null;
    }

    public void InstallComponent(Definitions.SHIPCOMPONENTS purchasedComponent, Definitions.DIRECTIONS installDirection)
    {
        EventManager.Instance.attachpointUnderMouse = null;
        myCollider.enabled = false;
        myBaseModel.SetActive(false);
        myHoverModel.SetActive(false);
        baseHideScript.enabled = false;

        EventManager.Instance.OnPlayerPickupComponent -= OnPlayerGrabComponent;
        EventManager.Instance.OnPlayerDropComponent -= OnPlayerDropComponent;

        AttachPoint attachPointScript = gameObject.GetComponent<AttachPoint>();
        switch (purchasedComponent)
        {
            case Definitions.SHIPCOMPONENTS.LASER:
                GameObject component = Instantiate(laserPrefab);
                attachPointScript.AttachComponent(component, installDirection);
                break;
            default:
            case Definitions.SHIPCOMPONENTS.NONE:
                Debug.Log("Empty Tile");
                break;
        }
    }
}
