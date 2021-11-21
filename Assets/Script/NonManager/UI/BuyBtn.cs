using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyBtn : MonoBehaviour
{
    public Button Button;
    public TextMeshProUGUI CostText;
    public TextMeshProUGUI GetText;
    public Image CostIcon;
    public Sprite CoinSprite;
    public Sprite JemSprite;
    public int JemCost;
    public int CoinCost;
    public int MoneyCost;
    public int GetJemCost;
    public int GetCoinCost;
    public int GetOvenCost;

    private void Start()
    {
        if (CoinCost > 0) { CostText.text = $"{CoinCost:#,0}"; CostIcon.sprite = CoinSprite; CostIcon.SetNativeSize(); CostIcon.rectTransform.sizeDelta /= 7; }
        else if (JemCost > 0) { CostText.text = $"{JemCost:#,0}"; CostIcon.sprite = JemSprite; CostIcon.SetNativeSize(); CostIcon.rectTransform.sizeDelta /= 10; }
        else if (MoneyCost > 0) { CostText.text = $"\\ {MoneyCost:#,0}"; Destroy(CostIcon.gameObject); };

        if (GetJemCost > 0) GetText.text = $"{GetJemCost:#,0}°³";
        else if (GetCoinCost > 0) GetText.text = $"{GetCoinCost:#,0}°³";
        else if (GetOvenCost > 0) GetText.text = $"{GetOvenCost:#,0}°³";

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
                if (JemCost <= GameManager.Instance.Jem)
                {
                    GameManager.Instance.Jem -= JemCost;
                    buy = true;
                }
            }
            else if (MoneyCost > 0)
            {
                buy = true;
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
