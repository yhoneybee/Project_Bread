using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class BuyBreadLinker : MonoBehaviour
{
    public static int Jem_one;
    public static int Jem_eleven;
    public static int Coin_one;
    public static int Coin_eleven;

    public StageInfoLinker.Reward_Kind CostType;
    public int value_one;
    public int value_eleven;

    public Sprite resourceIcon;

    public int rank_value;
    Button button;

    private void Start()
    {
        button = GetComponent<Button>();

        Coin_one = Coin_eleven = Jem_one = Jem_eleven = 0;

        button.onClick.AddListener(() =>
        {
            bool isUnboxing_one = false;
            bool isUnboxing_eleven = false;

            switch (CostType)
            {
                case StageInfoLinker.Reward_Kind.Coin:
                    if (GameManager.Instance.Coin >= value_one)
                    {
                        isUnboxing_one = true;
                        Coin_one = value_one;
                        ShopManager.Instance.txtOneCost.color = Color.white;
                    }
                    else
                    {
                        ShopManager.Instance.txtOneCost.color =
                        ShopManager.Instance.txtElevenCost.color = Color.red;
                        break;
                    }
                    if (GameManager.Instance.Coin >= value_eleven)
                    {
                        isUnboxing_eleven = true;
                        Coin_eleven = Jem_eleven;
                        ShopManager.Instance.txtElevenCost.color = Color.white;
                    }
                    else
                    {
                        ShopManager.Instance.txtElevenCost.color = Color.red;
                        break;
                    }
                    break;
                case StageInfoLinker.Reward_Kind.Jem:
                    if (GameManager.Instance.Jem >= value_one)
                    {
                        isUnboxing_one = true;
                        Jem_one = value_one;
                        ShopManager.Instance.txtElevenCost.color = Color.red;
                    }
                    else
                    {
                        ShopManager.Instance.txtOneCost.color =
                        ShopManager.Instance.txtElevenCost.color = Color.red;
                        break;
                    }
                    if (GameManager.Instance.Jem >= value_eleven)
                    {
                        isUnboxing_eleven = true;
                        Jem_eleven = value_eleven;
                        ShopManager.Instance.txtElevenCost.color = Color.white;
                    }
                    else
                    {
                        ShopManager.Instance.txtElevenCost.color = Color.red;
                        break;
                    }
                    break;
            }

            var one_text = ShopManager.Instance.txtOneCost;
            one_text.text = $"{value_one:#,0}";
            one_text.gameObject.SetActive(true);

            var eleven_text = ShopManager.Instance.txtElevenCost;
            eleven_text.text = $"{value_eleven:#,0}";

            foreach (var img in ShopManager.Instance.imgResourceIcon)
            {
                img.sprite = resourceIcon;
            }

            ShopManager.Instance.imgDropBox.sprite = ShopManager.Instance.imgBuyBox.sprite = ShopManager.Instance.BoxSprites[rank_value];
            UIManager.Instance.FixSizeToRatio(ShopManager.Instance.imgDropBox, 400);
            StartCoroutine(UIManager.Instance.EMovingUI(ShopManager.Instance.imgDropBox, new Vector2(-600, -260), 3000));
            if (isUnboxing_one || isUnboxing_eleven)
            {
                ShopManager.Instance.BuyWindow.gameObject.SetActive(true);

                ShopManager.Instance.Upper.sprite = ShopManager.Instance.UpperLowers[rank_value].Upper;
                ShopManager.Instance.Lower.sprite = ShopManager.Instance.UpperLowers[rank_value].Lower;

                ShopManager.Instance.btnOneBuy.onClick.RemoveAllListeners();
                ShopManager.Instance.btnElevenBuy.onClick.RemoveAllListeners();

                if (isUnboxing_one)
                {
                    ShopManager.Instance.btnOneBuy.onClick.AddListener(() =>
                    {
                        ShopManager.Instance.imgDropBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(-600, 1000);
                        ShopManager.Instance.Unboxing(true);
                        ButtonActions.Instance.UnBoxingOne(rank_value);
                    });
                }

                if (isUnboxing_eleven)
                {
                    ShopManager.Instance.btnElevenBuy.onClick.AddListener(() =>
                    {
                        ShopManager.Instance.imgDropBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(-600, 1000);
                        ShopManager.Instance.Unboxing(false);
                        ButtonActions.Instance.UnBoxingTen(rank_value);
                    });
                }
                else
                {
                    ShopManager.Instance.btnElevenBuy.onClick.AddListener(() =>
                    {
                        ShopManager.Instance.BuyWindow.gameObject.SetActive(false);
                        ShopManager.Instance.DontBuyWindow.gameObject.SetActive(true);
                    });
                }
            }
            else
            {
                ShopManager.Instance.BuyWindow.gameObject.SetActive(true);
                // TODO : µ·¾øÀ½ ¶ç¿ì±â
                ShopManager.Instance.btnOneBuy.onClick.RemoveAllListeners();
                ShopManager.Instance.btnOneBuy.onClick.AddListener(() =>
                {
                    ShopManager.Instance.BuyWindow.gameObject.SetActive(false);
                    ShopManager.Instance.DontBuyWindow.gameObject.SetActive(true);
                });

                ShopManager.Instance.btnElevenBuy.onClick.RemoveAllListeners();
                ShopManager.Instance.btnElevenBuy.onClick.AddListener(() =>
                {
                    ShopManager.Instance.BuyWindow.gameObject.SetActive(false);
                    ShopManager.Instance.DontBuyWindow.gameObject.SetActive(true);
                });
            }
        });
    }
}