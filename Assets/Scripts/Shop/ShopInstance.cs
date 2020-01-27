using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInstance : MonoBehaviour
{
    [Header("References")]
    public GameObject myShopSlotsContainer;
    public GameObject myModel;
    public GameObject shopSlotPrefab;
    public GameObject laserPrefab; // TODO refactor

    [Header("Settings")]
    public int shopHeight = 5;
    public int shopWidth = 5;

    private void Start()
    {
        if (myShopSlotsContainer == null || myModel == null || shopSlotPrefab == null || laserPrefab == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        myModel.transform.localScale = new Vector3(shopWidth, 1, shopHeight);
    }

    public void SetShopSlots(List<Tuple<int, Definitions.SHIPCOMPONENTS>> componentsToSet)
    {
        if (myShopSlotsContainer.transform.childCount > 0)
        {
            Debug.LogWarning("Shop already has components setup");
            return;
        }

        int componentCount = componentsToSet.Count;
        if (componentsToSet == null || componentCount <= 0)
        {
            Debug.LogWarning("Tried to set shop with empty component list");
            return;
        }

        int maxShopCap = (shopHeight * shopWidth);
        if (componentCount > maxShopCap)
        {
            Debug.LogWarning($"Tried to set {componentsToSet.Count} components in shop that can only hold {maxShopCap} components. Fitting what's possible...");
            componentCount = maxShopCap;
        }

        int rowCount = (int)Mathf.Ceil((float)componentCount / (float)shopHeight);
        float rowMod = Mathf.Floor((float)shopHeight / 2);
        float columnMod = Mathf.Floor((float)shopWidth / 2);

        Debug.Log("Filling shop...");
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < shopWidth; j++)
            {
                if (i + j >= componentCount)
                {
                    Debug.Log("Finished filling shop");
                    break;
                }
                GameObject newComponentSlot = Instantiate(shopSlotPrefab, myShopSlotsContainer.transform);
                newComponentSlot.transform.localPosition = new Vector3((j - columnMod), 0, (i - rowMod));
                PriceText componentPrice = newComponentSlot.GetComponent<PriceText>();

                if (componentPrice != null)
                {
                    if (componentsToSet[i + j].Item1 == 0)
                    {
                        componentPrice.ItemPriceString = "FREE";
                    } else
                    {
                        componentPrice.ItemPriceString = $"${componentsToSet[i + j].Item1}";
                    }   
                }

                if (componentsToSet[i + j].Item2 == Definitions.SHIPCOMPONENTS.LASER)
                {
                    GameObject newComponentForSale = Instantiate(laserPrefab, newComponentSlot.transform);
                }
            }
            
        }
    }
}
