using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyWindow : MonoBehaviour
{
    public BuyButton selected_button { get; set; }
    public ResourceShopLinker linker { get; set; }

    [SerializeField] TextMeshProUGUI product_amount_text;
    [SerializeField] TextMeshProUGUI product_price_text;
    [SerializeField] Image product_icon;
    [SerializeField] Image price_icon;
    [SerializeField] Button buy_button;
    void Start()
    {
        buy_button.onClick.AddListener(() =>
        {
            switch (selected_button.product_type)
            {
                case ProductType.COIN:
                    // Coin을 구매하기 위한 Jem이 충분히 있는지 확인
                    if (selected_button.product_price <= GameManager.Instance.Jem)
                    {
                        GameManager.Instance.Jem -= ((int)selected_button.product_price);
                        GameManager.Instance.Coin += ((int)selected_button.product_amount);
                        linker.buy_failed = false;
                    }
                    else linker.buy_failed = true;
                    break;

                case ProductType.JEM:
                    if (true)
                    {
                        GameManager.Instance.Jem += ((int)selected_button.product_amount);
                        linker.buy_failed = false;
                    }
                    else linker.buy_failed = true;
                    break;

                case ProductType.STEMINA:
                    // Stemina을 구매하기 위한 Jem이 충분히 있는지 확인
                    if (selected_button.price_type == PriceType.JEM)
                    {
                        if (selected_button.product_price <= GameManager.Instance.Jem)
                        {
                            GameManager.Instance.Jem -= ((int)selected_button.product_price);
                            linker.buy_failed = false;
                        }
                    }
                    // Stemina을 구매하기 위한 Coin이 충분히 있는지 확인
                    else if (selected_button.price_type == PriceType.COIN)
                    {
                        if (selected_button.product_price <= GameManager.Instance.Coin)
                        {
                            GameManager.Instance.Coin -= ((int)selected_button.product_price);
                            linker.buy_failed = false;
                        }
                        else
                        {
                            linker.buy_failed = true;
                            break;
                        }
                    }

                    GameManager.Instance.Stemina += ((int)selected_button.product_amount);
                    break;
            }

            gameObject.SetActive(false);
        });
    }

    public void Setting()
    {
        gameObject.SetActive(true);

        product_amount_text.text = "X " + selected_button.amount_text.text;
        switch (selected_button.price_type)
        {
            case PriceType.COIN:
                product_price_text.color = GameManager.Instance.Coin >=
                    selected_button.product_price ? Color.white : Color.red;
                break;
            case PriceType.JEM:
                product_price_text.color = GameManager.Instance.Jem >=
                            selected_button.product_price ? Color.white : Color.red;
                break;
            case PriceType.WON:
                product_price_text.color = Color.white;
                break;
        }

        price_icon.enabled = true;

        product_icon.sprite = linker.icon_sprites[(int)selected_button.product_type];
        product_icon.SetNativeSize();

        if (selected_button.product_type == ProductType.JEM)
            product_icon.rectTransform.sizeDelta /= 2;
        else if (selected_button.product_type == ProductType.STEMINA)
            product_icon.rectTransform.sizeDelta *= 2;

        price_icon.sprite = selected_button.price_image.sprite;
        price_icon.SetNativeSize();

        price_icon.rectTransform.sizeDelta /=
            (selected_button.price_type == PriceType.JEM ? 11f : 7f);

        if (selected_button.price_type == PriceType.WON)
            price_icon.enabled = false;

        buy_button.GetComponentInChildren<TextMeshProUGUI>().text = selected_button.price_text.text;
    }

    void Update()
    {
    }
}
