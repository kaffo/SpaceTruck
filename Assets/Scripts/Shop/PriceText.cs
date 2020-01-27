using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PriceText : MonoBehaviour
{
    public TextMeshProUGUI itemPriceText;

    private string itemPriceString = "$100";
    public string ItemPriceString
    {
        get { return itemPriceString; }
        set {
            itemPriceString = value;
            itemPriceText.text = value;
        }
    }

    private void Start()
    {
        if (itemPriceText == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        itemPriceText.text = itemPriceString;
    }
}
