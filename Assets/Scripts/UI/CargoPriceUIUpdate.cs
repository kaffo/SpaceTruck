using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CargoPriceUIUpdate : MonoBehaviour
{
    public List<TextMeshProUGUI> cargoPriceTexts;

    private int cargoPrice = 500;
    public int CargoPrice
    {
        set
        {
            cargoPrice = value;
            SetCargoTexts();
        }
        get { return cargoPrice; }
    }

    private void Start()
    {
        if (cargoPriceTexts == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        SetCargoTexts();
    }

    private void SetCargoTexts()
    {
        foreach (TextMeshProUGUI cargoPriceText in cargoPriceTexts)
        {
            cargoPriceText.text = $"{cargoPrice}";
        }
    }
}
