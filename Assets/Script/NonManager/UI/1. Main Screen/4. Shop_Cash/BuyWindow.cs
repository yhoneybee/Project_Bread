using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyWindow : MonoBehaviour
{
    public CashProduct selecrted_product { get; set; }
    public CashShopLinker linker { get; set; }

    [SerializeField] TextMeshProUGUI product_amount_text;
    [SerializeField] TextMeshProUGUI product_price_text;
    [SerializeField] Image product_icon;
    [SerializeField] Image price_icon;
    [SerializeField] Button buy_button;
    void Start()
    {
        buy_button.onClick.AddListener(() =>
        {
            switch (selecrted_product.product_type)
            {
                case ProductType.COIN:
                    // Coin을 구매하기 위한 Jem이 충분히 있는지 확인
                    if (selecrted_product.product_price <= GameManager.Instance.Jem)
                    {
                        GameManager.Instance.Jem -= ((int)selecrted_product.product_price);
                        GameManager.Instance.Coin += ((int)selecrted_product.product_amount);
                        linker.buy_failed = false;
                    }
                    else linker.buy_failed = true;
                    break;

                case ProductType.JEM:
                    if (true)
                    {
                        GameManager.Instance.Jem += ((int)selecrted_product.product_amount);
                        linker.buy_failed = false;
                    }
                    else linker.buy_failed = true;
                    break;

                case ProductType.STEMINA:
                    // Stemina을 구매하기 위한 Jem이 충분히 있는지 확인
                    if (selecrted_product.price_type == PriceType.JEM)
                    {
                        if (selecrted_product.product_price <= GameManager.Instance.Jem)
                        {
                            GameManager.Instance.Jem -= ((int)selecrted_product.product_price);
                            linker.buy_failed = false;
                        }
                    }
                    // Stemina을 구매하기 위한 Coin이 충분히 있는지 확인
                    else if (selecrted_product.price_type == PriceType.COIN)
                    {
                        if (selecrted_product.product_price <= GameManager.Instance.Coin)
                        {
                            GameManager.Instance.Coin -= ((int)selecrted_product.product_price);
                            linker.buy_failed = false;
                        }
                        else
                        {
                            linker.buy_failed = true;
                            break;
                        }
                    }

                    GameManager.Instance.Stemina += ((int)selecrted_product.product_amount);
                    break;
            }

            gameObject.SetActive(false);
        });
    }

    public void Setting()
    {
        gameObject.SetActive(true);

        product_amount_text.text = "X " + selecrted_product.amount_text.text;
        switch (selecrted_product.price_type)
        {
            case PriceType.COIN:
                product_price_text.color = GameManager.Instance.Coin >=
                    selecrted_product.product_price ? Color.white : Color.red;
                break;
            case PriceType.JEM:
                product_price_text.color = GameManager.Instance.Jem >=
                            selecrted_product.product_price ? Color.white : Color.red;
                break;
            case PriceType.WON:
                product_price_text.color = Color.white;
                break;
        }

        price_icon.enabled = true;

        product_icon.sprite = linker.icon_sprites[(int)selecrted_product.product_type];
        product_icon.SetNativeSize();

        if (selecrted_product.product_type == ProductType.JEM)
            product_icon.rectTransform.sizeDelta /= 2;
        else if (selecrted_product.product_type == ProductType.STEMINA)
            product_icon.rectTransform.sizeDelta *= 2;

        price_icon.sprite = selecrted_product.price_image.sprite;
        price_icon.SetNativeSize();

        price_icon.rectTransform.sizeDelta /=
            (selecrted_product.price_type == PriceType.JEM ? 11f : 7f);

        if (selecrted_product.price_type == PriceType.WON)
            price_icon.enabled = false;

        buy_button.GetComponentInChildren<TextMeshProUGUI>().text = selecrted_product.price_text.text;
    }

    void Update()
    {
    }
}
