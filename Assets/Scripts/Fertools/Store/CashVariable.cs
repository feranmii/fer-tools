using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Fertools.Store
{
    

[CreateAssetMenu(menuName = "Store/Cash")]
public class CashVariable : ScriptableObject
{
    private const int MAX_CASH = 999999;
    public int cash;

    public void AddCash(int value)
    {
        if (cash <= MAX_CASH)
            cash += value;
    }

    public void RemoveCash(int value)
    {
        cash -= value;

        if (cash < 0)
            cash = 0;
    }

    public bool AttemptPurchase(int cost)
    {
        var retVal = false;
        
        if (cash >= cost)
        {
            RemoveCash(cost);
            retVal = true;
            Debug.Log("Purchase Coomplete");
        }
        else
        {
            retVal = false;
            Debug.Log("Insufficient Cash");
        }

        return retVal;
    }
    
    /// <summary>
    /// Sync cash with the storage
    /// </summary>
    public void RefreshCash()
    {
        cash = LocalStorage.Cash;
    }

    public void SaveCash()
    {
        LocalStorage.Cash = cash;
    }
}

}