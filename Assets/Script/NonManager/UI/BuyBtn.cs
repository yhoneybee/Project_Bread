using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyBtn : MonoBehaviour
{
    public Button Button;
    public TextMeshProUGUI CostText;
    public int JemCost;
    public int CoinCost;
    public int MoneyCost;
    public int GetJemCost;
    public int GetCoinCost;
    public int GetOvenCost;

    private void Start()
    {
        string val = "";

        if (CoinCost > 0) val = $"{CoinCost} COIN";
        else if (JemCost > 0) val = $"{JemCost} JEM";
        else if (MoneyCost > 0) val = $"{MoneyCost} WON";

        CostText.text = val;

        Button.onClick.AddListener(() =>
        {
            bool buy = false;
            if (CoinCost > 0)
            {
                if (CoinCost <= GameManager.Instance.Coin)
                {
                    GameManager.Instance.Coin -= CoinCost;
                    buy = true;
                }
            }
            else if (JemCost > 0)
            {
                if (CoinCost <= GameManager.Instance.Jem)
                {
                    GameManager.Instance.Jem -= JemCost;
                    buy = true;
                }
            }
            if (buy)
            {
                GameManager.Instance.Coin += GetCoinCost;
                GameManager.Instance.Jem += GetJemCost;
                GameManager.Instance.Stemina += GetOvenCost;
            }
        });
    }
}
