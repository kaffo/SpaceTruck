using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUIUpdate : MonoBehaviour
{
    public Text moneyText;

    private int playerMoney = 1000;
    public int PlayerMoney
    {
        set
        {
            playerMoney = value;
            moneyText.text = $"${playerMoney}";
        }
        get { return playerMoney; }
    }

    private void Start()
    {
        if (moneyText == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        moneyText.text = $"${playerMoney}";
    }

    private void OnEnable()
    {
        MoneyManager.Instance.OnPlayerMoneyChange += OnPlayerMoneyChange;
    }

    private void OnDisable()
    {
        if (MoneyManager.Instance != null) 
            MoneyManager.Instance.OnPlayerMoneyChange -= OnPlayerMoneyChange;
    }

    private void OnPlayerMoneyChange(int newTotal)
    {
        PlayerMoney = newTotal;
    }
}
