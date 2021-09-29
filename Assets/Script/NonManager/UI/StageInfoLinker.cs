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

        GameObject obj = Instantiate(ClearPrefab);
        obj.GetComponent<RectTransform>().SetParent(RewardContent, false);

        if (reward.FirstClear.Item3)
        {
            obj.GetComponent<Image>().sprite = null;
            obj.GetComponentInChildren<TextMeshProUGUI>().text = $"{reward.FirstClear.Item3}";
        }

        obj = Instantiate(ClearPrefab);
        obj.GetComponent<RectTransform>().SetParent(RewardContent, false);

        if (reward.FirstClear.Item1 > 0)
        {
            obj.GetComponent<Image>().sprite = null;
            obj.GetComponentInChildren<TextMeshProUGUI>().text = $"{reward.FirstClear.Item1}";
        }

        obj = Instantiate(ClearPrefab);
        obj.GetComponent<RectTransform>().SetParent(RewardContent, false);

        if (reward.FirstClear.Item2 > 0)
        {
            obj.GetComponent<Image>().sprite = null;
            obj.GetComponentInChildren<TextMeshProUGUI>().text = $"{reward.FirstClear.Item2}";
        }
    }
}
