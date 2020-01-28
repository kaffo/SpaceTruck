using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : Singleton<MoneyManager>
{
    private int playerMoney = 1000;
    public int PlayerMoney
    {
        get { return playerMoney; }
    }

    public delegate void OnPlayerMoneyChangeDelegate(int newTotal);
    public event OnPlayerMoneyChangeDelegate OnPlayerMoneyChange;

    public bool CanAfford(int price)
    {
        return (playerMoney - price >= 0);
    }

    public bool Purchase(int price)
    {
        if (CanAfford(price))
        {
            playerMoney -= price;
            Debug.Log($"New player money ${playerMoney}");
            OnPlayerMoneyChange?.Invoke(playerMoney);
            return true;
        } else
        {
            return false;
        }
    }

    public void Reward(int amount)
    {
        playerMoney += amount;
        Debug.Log($"New player money ${playerMoney}");
        OnPlayerMoneyChange?.Invoke(playerMoney);
    }
}
