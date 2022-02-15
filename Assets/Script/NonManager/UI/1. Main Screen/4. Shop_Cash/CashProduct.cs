using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CashProduct : MonoBehaviour
{
    public Image price_image;

    public Button buy_button;

    public TextMeshProUGUI price_text;
    public TextMeshProUGUI amount_text;

    public int product_amount;

    public float product_price;

    public ProductType product_type;
    public PriceType price_type;

    public void GetReward()
    {
        switch (product_type)
        {
            case ProductType.COIN:
                GameManager.Instance.Coin += product_amount;
                break;
            case ProductType.JEM:
                GameManager.Instance.Jem += product_amount;
                break;
            case ProductType.STEMINA:
                GameManager.Instance.Stemina += product_amount;
                break;
        }
    }


    /*    public Button Button;
        public TextMeshProUGUI CostText;
        public TextMeshProUGUI GetText;
        public Image CostIcon;
        public Sprite CoinSprite;
        public Sprite JemSprite;
        public Sprite SteminaSprite;
        public int JemCost;
        public int CoinCost;
        public int MoneyCost;
        public int GetJemCost;
        public int GetCoinCost;
        public int GetOvenCost;*/
    /*
        private void Start()
        {
            if (CoinCost > 0) { CostText.text = $"{CoinCost:#,0}"; CostIcon.sprite = CoinSprite; CostIcon.SetNativeSize(); CostIcon.rectTransform.sizeDelta /= 7; }
            else if (JemCost > 0) { CostText.text = $"{JemCost:#,0}"; CostIcon.sprite = JemSprite; CostIcon.SetNativeSize(); CostIcon.rectTransform.sizeDelta /= 10; }
            else if (MoneyCost > 0) { CostText.text = $"\\ {MoneyCost:#,0}"; Destroy(CostIcon.gameObject); };

            if (GetJemCost > 0) GetText.text = $"{GetJemCost:#,0}개";
            else if (GetCoinCost > 0) GetText.text = $"{GetCoinCost:#,0}개";
            else if (GetOvenCost > 0) GetText.text = $"{GetOvenCost:#,0}개";

            Button.onClick.AddListener(() =>
            {
                buy_window.buy_window.SetActive(true);
                if (CoinCost > 0)
                {
                    buy_window.product_amount_text.text = $"X {CoinCost:#,0}개";
                    buy_window.icon_image.sprite = SteminaSprite;
                }
                else if (JemCost > 0)
                {
                    buy_window.product_amount_text.text = $"X {JemCost:#,0}개";
                    buy_window.icon_image.sprite = CoinSprite;
                }
                else if (MoneyCost > 0)
                {
                    buy_window.product_amount_text.text = $"X {MoneyCost:#,0}개";
                    buy_window.icon_image.sprite = JemSprite;
                }
            });

            buy_window.buy_button.onClick.AddListener(() =>
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

        private void Update()
        {
            if (CoinCost > 0)
            {
                if (GameManager.Instance.Coin < CoinCost)
                    CostText.color = Color.red;
            }
            else if (JemCost > 0)
            {
                if (GameManager.Instance.Jem < JemCost)
                    CostText.color = Color.red;
            }
            //else if (MoneyCost > 0) if (GameManager.Instance.Money < MoneyCost) CostText.color = Color.red;
        }*/
}
