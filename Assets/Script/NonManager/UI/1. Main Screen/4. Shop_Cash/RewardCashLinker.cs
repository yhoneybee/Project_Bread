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

    private void Start()
    {
        price_text.text = $"(3/3)";
        amount_text.text = $"{product_amount:#,0}개";

        buy_button.onClick.AddListener(() => 
        {
            // 여기서 AdmobManager의 보상값 조정
            AdmobManager.Instance.rewardKind = product_type;
            AdmobManager.Instance.rewardValue = product_amount;
            AdmobManager.Instance.ShowRewardedAd();
        });
    }
}
