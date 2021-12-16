using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum eSPAWN_COUNT
{
    TEN,
    ONE,
}

public class SpawnBtnLinker : MonoBehaviour
{
    public static int Jem;
    public static int Coin;

    public StageInfoLinker.Reward_Kind CostType;
    public int value;

    public TextMeshProUGUI txtReward;
    public Image imgIcon;

    public eSPAWN_COUNT eSpawnCount;
    public int rank_value;
    Button button;

    private void Start()
    {
        imgIcon.sprite = UIManager.Instance.spIcon[((int)CostType)];
        txtReward.text = $"{value:#,0}";

        button = GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            bool isUnboxing = false;
            imgIcon.sprite = UIManager.Instance.spIcon[((int)CostType)];
            switch (CostType)
            {
                case StageInfoLinker.Reward_Kind.Coin:

                    if (GameManager.Instance.Coin >= value)
                    {
                        isUnboxing = true;
                        Coin = value;
                    }
                    break;
                case StageInfoLinker.Reward_Kind.Jem:
                    if (GameManager.Instance.Jem >= value)
                    {
                        isUnboxing = true;
                        Jem = value;
                    }
                    break;
            }

            foreach (var txt in ShopManager.Instance.txtCost)
            {
                txt.text = $"{value:#,0}";
                txt.gameObject.SetActive(true);
            }

            ShopManager.Instance.imgResourceIcon.sprite = imgIcon.sprite;

            ShopManager.Instance.imgDropBox.sprite = ShopManager.Instance.imgBuyBox.sprite = ShopManager.Instance.BoxSprites[rank_value];
            UIManager.Instance.FixSizeToRatio(ShopManager.Instance.imgDropBox, 400);
            StartCoroutine(UIManager.Instance.EMovingUI(ShopManager.Instance.imgDropBox, new Vector2(-600, -260), 3000));
            if (isUnboxing)
            {
                ShopManager.Instance.txtCost[1].gameObject.SetActive(false);

                ShopManager.Instance.Upper.sprite = ShopManager.Instance.UpperLowers[rank_value].Upper;
                ShopManager.Instance.Lower.sprite = ShopManager.Instance.UpperLowers[rank_value].Lower;
                UIManager.Instance.FixSizeToRatio(ShopManager.Instance.imgBuyBox, 300);


                switch (eSpawnCount)
                {
                    case eSPAWN_COUNT.TEN:
                        ButtonActions.Instance.UnBoxingTen(rank_value);
                        break;
                    case eSPAWN_COUNT.ONE:
                        ButtonActions.Instance.UnBoxingOne(rank_value);
                        break;
                }
                ShopManager.Instance.btnBuy.onClick.RemoveAllListeners();
                ShopManager.Instance.btnBuy.onClick.AddListener(() =>
                {
                    ShopManager.Instance.imgDropBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(-600, 1000);
                    ShopManager.Instance.Unboxing();
                });
            }
            else
            {
                ShopManager.Instance.BuyWindow.gameObject.SetActive(true);
                // TODO : µ·¾øÀ½ ¶ç¿ì±â
                ShopManager.Instance.btnBuy.onClick.RemoveAllListeners();
                ShopManager.Instance.btnBuy.onClick.AddListener(() =>
                {
                    ShopManager.Instance.BuyWindow.gameObject.SetActive(false);
                    ShopManager.Instance.DontBuyWindow.gameObject.SetActive(true);
                });
            }
        });
    }
}
