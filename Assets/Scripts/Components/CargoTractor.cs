using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CargoTractor : MonoBehaviour
{
    public GameObject myMountPoint;
    private Collider myCollider;
    private CargoContainer myCargoContainer;

    private void Start()
    {
        if (myMountPoint == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        myCollider = gameObject.GetComponent<Collider>();
        myCollider.enabled = false;

        GameManager.Instance.OnRunStart += OnRunStart;
        GameManager.Instance.OnRunEnd += OnRunEnd;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnRunStart -= OnRunStart;
            GameManager.Instance.OnRunEnd -= OnRunEnd;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CargoContainer cargoContainerScript = other.GetComponent<CargoContainer>();
        if (cargoContainerScript != null)
        {
            cargoContainerScript.CaptureCargo();
            Transform cargoContainerTransform = cargoContainerScript.transform;
            cargoContainerTransform.SetParent(myMountPoint.transform);
            cargoContainerTransform.localPosition = Vector3.zero;
            cargoContainerTransform.rotation = Quaternion.identity;

            myCargoContainer = cargoContainerScript;

            myCollider.enabled = false;
        }
    }

    private void OnRunStart()
    {
        myCollider.enabled = true;
    }

    private void OnRunEnd()
    {
        myCollider.enabled = false;

        // If we have cargo, reward the player
        if (myCargoContainer != null)
        {
            MoneyManager.Instance.Reward(myCargoContainer.myPrice);
            Destroy(myCargoContainer.gameObject);
            myCargoContainer = null;
        }
    }
}
