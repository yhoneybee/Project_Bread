using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyBtn : MonoBehaviour
{
    public Button Button;
    public int BuyJemCost;
    public int BuyCoinCost;
    public int GetJemCost;
    public int GetCoinCost;
    public int GetOvenCost;

    private void Start()
    {
        Button.onClick.AddListener(() => 
        {
            
        });
    }
}
