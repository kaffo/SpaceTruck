using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CargoPriceUIUpdate))]
public class CargoContainer : MonoBehaviour
{
    public int myPrice = 500;

    private void OnEnable()
    {
        gameObject.GetComponent<CargoPriceUIUpdate>().CargoPrice = myPrice;
    }

    public void CaptureCargo()
    {
        AsteroidMove asteroidMove = gameObject.GetComponent<AsteroidMove>();

        if (asteroidMove != null)
            Destroy(asteroidMove);
        else
            Debug.LogWarning("Couldn't find move script on Cargo Container");
    }
}
