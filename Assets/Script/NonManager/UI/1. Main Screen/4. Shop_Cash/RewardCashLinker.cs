using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardCashLinker : MonoBehaviour
{
    public Button buy_button;

    public TextMeshProUGUI price_text;
    public TextMeshProUGUI amount_text;

    public int product_amount;

    public ProductType product_type;

    private void OnEnable()
    {
        price_text.text = $"({product_type switch { ProductType.JEM => GameManager.Instance.RewardCount.JemRewardCount, ProductType.COIN => GameManager.Instance.RewardCount.CoinRewardCount, ProductType.STEMINA => GameManager.Instance.RewardCount.SteminaRewardCount, _ => -1, }}/3)";
        amount_text.text = $"{product_amount:#,0}개";
    }

    private void Start()
    {
        buy_button.onClick.AddListener(() =>
        {
            // 여기서 AdmobManager의 보상값 조정
            switch (product_type)
            {
                case ProductType.COIN:
                    if (GameManager.Instance.RewardCount.CoinRewardCount <= 0) return;
                    GameManager.Instance.RewardCount.CoinRewardCount--;
                    break;
                case ProductType.JEM:
                    if (GameManager.Instance.RewardCount.JemRewardCount <= 0) return;
                    GameManager.Instance.RewardCount.JemRewardCount--;
                    break;
                case ProductType.STEMINA:
                    if (GameManager.Instance.RewardCount.SteminaRewardCount <= 0) return;
                    GameManager.Instance.RewardCount.SteminaRewardCount--;
                    break;
            }

            OnEnable();

            AdmobManager.Instance.rewardKind = product_type;
            AdmobManager.Instance.rewardValue = product_amount;
            //AdmobManager.Instance.ShowRewardedAd();
        });
    }
}
