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
    private Definitions.DIRECTIONS myDirection = Definitions.DIRECTIONS.FORWARD;

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

    private void Update()
    {
        if (grabbed)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.transform.position.y - myItemSlot.transform.position.y;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

            myItemSlot.transform.position = mouseWorldPos;

            // Rotate Left
            if (Input.GetKeyDown(KeyCode.Q))
            {
                myItemSlot.transform.Rotate(new Vector3(0, -90, 0));
                if (myDirection == Definitions.DIRECTIONS.FORWARD)
                {
                    myDirection = Definitions.DIRECTIONS.LEFT;
                } else
                {
                    myDirection--;
                }
            }

            // Rotate Left
            if (Input.GetKeyDown(KeyCode.E))
            {
                myItemSlot.transform.Rotate(new Vector3(0, 90, 0));
                if (myDirection == Definitions.DIRECTIONS.LEFT)
                {
                    myDirection = Definitions.DIRECTIONS.FORWARD;
                }
                else
                {
                    myDirection++;
                }
            }
        }
    }

    public void RotateComponent(Definitions.DIRECTIONS newDirection)
    {
        switch (newDirection)
        {
            case Definitions.DIRECTIONS.FORWARD:
                myItemSlot.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;
            case Definitions.DIRECTIONS.RIGHT:
                myItemSlot.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                break;
            case Definitions.DIRECTIONS.BACKWARD:
                myItemSlot.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                break;
            case Definitions.DIRECTIONS.LEFT:
                myItemSlot.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                break;
            default:
                myItemSlot.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;
        }
        myDirection = newDirection;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnPlayerPickupComponent += OnGrabComponentStart;
        EventManager.Instance.OnPlayerDropComponent += OnGrabComponentEnd;
    }

    private void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnPlayerPickupComponent -= OnGrabComponentStart;
            EventManager.Instance.OnPlayerDropComponent -= OnGrabComponentEnd;
        }
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

        // Offset code for grabbing object, might reintroduce later
        /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
            grabOffset = hitInfo.point - transform.position;*/
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

            EventManager.Instance.OnPlayerPickupComponent -= OnGrabComponentStart;
            EventManager.Instance.OnPlayerDropComponent -= OnGrabComponentEnd;

            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponent<PriceText>().ItemPriceString = "SOLD";
            DeleteSlotItems();
            EventManager.Instance.attachpointUnderMouse.InstallComponent(myShipComponent, myDirection);
            myBaseModel.SetActive(false);
        } else
        {
            myItemSlot.transform.localPosition = Vector3.zero;
        }
    }

    private void OnGrabComponentStart()
    {
        myBaseModel.SetActive(false);
    }

    private void OnGrabComponentEnd()
    {
        Debug.Log($"{gameObject.name} grab end");
        myBaseModel.SetActive(true);
    }
}
