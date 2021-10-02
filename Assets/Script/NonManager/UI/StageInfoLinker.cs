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
        RewardInfo reward = StageManager.Instance.GetReward();
        AddRewards(reward.FirstClear);
        AddRewards(reward.ThreeStarClear);
        AddRewards(reward.Clear);

        AddWaveDatas();

        RewardContent.sizeDelta = new Vector2(-848 + ((165 * RewardContent.childCount) + 30), RewardContent.sizeDelta.y);
        MobContent.sizeDelta = new Vector2(-848 + ((165 * MobContent.childCount) + 30), MobContent.sizeDelta.y);
    }

    public void AddRewards(Tuple<int, int, Unit> clear)
    {
        GameObject obj = null;

        if (clear.Item3)
        {
            obj = Instantiate(ClearPrefab);
            obj.name = "Unit";
            obj.GetComponent<RectTransform>().SetParent(RewardContent, false);
            obj.GetComponent<Image>().sprite = null;
            obj.GetComponentInChildren<TextMeshProUGUI>().text = $"{clear.Item3}";
        }

        if (clear.Item1 > 0)
        {
            obj = Instantiate(ClearPrefab);
            obj.name = "Jem";
            obj.GetComponent<RectTransform>().SetParent(RewardContent, false);
            obj.GetComponent<Image>().sprite = null;
            obj.GetComponentInChildren<TextMeshProUGUI>().text = $"{clear.Item1}";
        }

        if (clear.Item2 > 0)
        {
            obj = Instantiate(ClearPrefab);
            obj.name = "Coin";
            obj.GetComponent<RectTransform>().SetParent(RewardContent, false);
            obj.GetComponent<Image>().sprite = null;
            obj.GetComponentInChildren<TextMeshProUGUI>().text = $"{clear.Item2}";
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
