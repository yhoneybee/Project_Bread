using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum ProductType
{
    COIN,
    JEM,
    STEMINA
}
public enum PriceType
{
    COIN,
    JEM,
    WON
}

public class CashShopLinker : MonoBehaviour
{
    [SerializeField] CashProduct[] cash_products;

    public Sprite[] icon_sprites =
    {
         null, // Coin Sprite
         null, // Jem Sprite
         null // Stemina Sprite
    };

    [SerializeField] BuyWindow buy_window;

    [SerializeField] GameObject DontBuyWindow;

    private bool _buy_failed;
    public bool buy_failed
    {
        get => _buy_failed;
        set
        {
            _buy_failed = value;
            DontBuyWindow.SetActive(true);
            if (_buy_failed)
                DontBuyWindow.GetComponentInChildren<TextMeshProUGUI>().text =
                    "재화가 부족합니다.\n재화를 확인해주세요.";
            else
                DontBuyWindow.GetComponentInChildren<TextMeshProUGUI>().text =
                    "구매에 성공했습니다!";
            _buy_failed = false;
        }
    }

    void Start()
    {
        buy_window.linker = this;

        foreach (var cash_product in cash_products)
        {
            switch (cash_product.product_type)
            {
                case ProductType.COIN:
                    cash_product.price_image.sprite = icon_sprites[1];
                    cash_product.price_image.SetNativeSize();
                    cash_product.price_image.rectTransform.sizeDelta /= 11f;
                    break;

                case ProductType.JEM:
                    //buy_window.price_icon.sprite = icon_sprites[3];
                    cash_product.price_image.enabled = false;
                    break;

                case ProductType.STEMINA:

                    if (cash_product.price_type == PriceType.COIN)
                    {
                        cash_product.price_image.sprite = icon_sprites[0];
                        cash_product.price_image.SetNativeSize();
                        cash_product.price_image.rectTransform.sizeDelta /= 7f;
                    }
                    else if (cash_product.price_type == PriceType.JEM)
                    {
                        cash_product.price_image.sprite = icon_sprites[1];
                        cash_product.price_image.SetNativeSize();
                        cash_product.price_image.rectTransform.sizeDelta /= 11f;
                    }
                    break;
            }

            cash_product.price_text.text = $"{cash_product.product_price:#,0}";
            if (cash_product.product_type == ProductType.JEM)
                cash_product.price_text.text = "\\ " + cash_product.price_text.text;
            cash_product.amount_text.text = $"{cash_product.product_amount:#,0}개";
            cash_product.buy_button.onClick.AddListener(() =>
            {
                buy_window.selecrted_product = cash_product;
                buy_window.Setting();
            });
        }
    }

    void Update()
    {
    }
}
