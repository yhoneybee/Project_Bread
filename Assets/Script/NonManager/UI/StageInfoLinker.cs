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
        RewardInformation reward = StageManager.Instance.GetReward();
        AddRewards(reward.FirstClear);
        AddRewards(reward.ThreeStarClear);
        AddRewards(reward.Clear);

        if (MobContent)
        {
            AddWaveDatas();

            MobContent.sizeDelta = new Vector2(-848 + ((165 * MobContent.childCount) + 30), MobContent.sizeDelta.y);
        }
        RewardContent.sizeDelta = new Vector2(-848 + ((165 * RewardContent.childCount) + 30), RewardContent.sizeDelta.y);
    }

    public void AddRewards(RewardInformation.Reward_Information clear)
    {
        GameObject obj = null;

        if (clear.unit)
        {
            obj = Instantiate(ClearPrefab);
            obj.name = "Unit";
            obj.GetComponent<RectTransform>().SetParent(RewardContent, false);
            obj.GetComponent<Image>().sprite = null;
            obj.GetComponentInChildren<TextMeshProUGUI>().text = $"{clear.unit}";
        }

        if (clear.jem > 0)
        {
            obj = Instantiate(ClearPrefab);
            obj.name = "Jem";
            obj.GetComponent<RectTransform>().SetParent(RewardContent, false);
            obj.GetComponent<Image>().sprite = null;
            obj.GetComponentInChildren<TextMeshProUGUI>().text = $"{clear.jem}";
        }

        if (clear.coin > 0)
        {
            obj = Instantiate(ClearPrefab);
            obj.name = "Coin";
            obj.GetComponent<RectTransform>().SetParent(RewardContent, false);
            obj.GetComponent<Image>().sprite = null;
            obj.GetComponentInChildren<TextMeshProUGUI>().text = $"{clear.coin}";
        }
    }

    public void AddWaveDatas()
    {
        var wave_data = StageManager.Instance.GetWaveData();
        GameObject obj = null;

        foreach (var wave in wave_data.wave_information)
        {
            if (wave.unit)
            {
                obj = Instantiate(ClearPrefab);
                obj.name = $"{wave.unit.Info.Name}";
                obj.GetComponent<RectTransform>().SetParent(MobContent, false);
                obj.GetComponent<Image>().sprite = wave.unit.Info.Icon;
            }
        }
    }
}
