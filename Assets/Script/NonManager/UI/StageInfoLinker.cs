using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StageInfoLinker : MonoBehaviour
{
    [SerializeField] GameObject ClearPrefab;

    [SerializeField] RectTransform MobContent;
    [SerializeField] RectTransform RewardContent;

    private void Start()
    {
        AddRewards();
    }

    public void AddRewards()
    {
        RewardInfo reward = StageManager.Instance.GetReward();

        GameObject obj = null;

        if (reward.FirstClear.Item3)
        {
            obj = Instantiate(ClearPrefab);
            obj.name = "F Unit";
            obj.GetComponent<RectTransform>().SetParent(RewardContent, false);
            obj.GetComponent<Image>().sprite = null;
            obj.GetComponentInChildren<TextMeshProUGUI>().text = $"{reward.FirstClear.Item3}";
        }

        if (reward.FirstClear.Item1 > 0)
        {
            obj = Instantiate(ClearPrefab);
            obj.name = "F Jem";
            obj.GetComponent<RectTransform>().SetParent(RewardContent, false);
            obj.GetComponent<Image>().sprite = null;
            obj.GetComponentInChildren<TextMeshProUGUI>().text = $"{reward.FirstClear.Item1}";
        }

        if (reward.FirstClear.Item2 > 0)
        {
            obj = Instantiate(ClearPrefab);
            obj.name = "F Coin";
            obj.GetComponent<RectTransform>().SetParent(RewardContent, false);
            obj.GetComponent<Image>().sprite = null;
            obj.GetComponentInChildren<TextMeshProUGUI>().text = $"{reward.FirstClear.Item2}";
        }
    }
}
