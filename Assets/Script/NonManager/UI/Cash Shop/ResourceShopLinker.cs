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

public class ResourceShopLinker : MonoBehaviour
{
    [SerializeField] BuyButton[] buy_buttons;

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

        foreach (var buy_button in buy_buttons)
        {
            switch (buy_button.product_type)
            {
                case ProductType.COIN:
                    buy_button.price_image.sprite = icon_sprites[1];
                    buy_button.price_image.SetNativeSize();
                    buy_button.price_image.rectTransform.sizeDelta /= 11f;
                    break;

                case ProductType.JEM:
                    //buy_window.price_icon.sprite = icon_sprites[3];
                    buy_button.price_image.enabled = false;
                    break;

                case ProductType.STEMINA:

                    if (buy_button.price_type == PriceType.COIN)
                    {
                        buy_button.price_image.sprite = icon_sprites[0];
                        buy_button.price_image.SetNativeSize();
                        buy_button.price_image.rectTransform.sizeDelta /= 7f;
                    }
                    else if (buy_button.price_type == PriceType.JEM)
                    {
                        buy_button.price_image.sprite = icon_sprites[1];
                        buy_button.price_image.SetNativeSize();
                        buy_button.price_image.rectTransform.sizeDelta /= 11f;
                    }
                    break;
            }

            buy_button.price_text.text = $"{buy_button.product_price:#,0}";
            if (buy_button.product_type == ProductType.JEM)
                buy_button.price_text.text = "\\ " + buy_button.price_text.text;
            buy_button.amount_text.text = $"{buy_button.product_amount:#,0}개";
            buy_button.buy_button.onClick.AddListener(() =>
            {
                buy_window.selected_button = buy_button;
                buy_window.Setting();
            });
        }
    }

    void Update()
    {
    }
}
