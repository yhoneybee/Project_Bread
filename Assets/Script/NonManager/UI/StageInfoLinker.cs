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

    [SerializeField] Sprite coin_sprite = null;
    [SerializeField] Sprite jem_sprite = null;

    [SerializeField] float image_devide_value;

    bool is_played = false;

    private void Start()
    {
        SetRewards();
    }

    public void SetRewards(bool first = true, bool three_star = true)
    {
        if (is_played) return;
        is_played = true;
        RewardInformation reward = StageManager.Instance.GetReward();
        if (first)
            AddRewards(reward.first_clear, 1);
        if (three_star)
            AddRewards(reward.three_star_clear, 2);
        AddRewards(reward.basic_clear);

        if (MobContent)
        {
            AddWaveDatas();

            var MobGLG = MobContent.GetComponent<GridLayoutGroup>();
            MobContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, MobContent.sizeDelta.x + (((MobGLG.cellSize.x + MobGLG.spacing.x) * MobContent.childCount) + MobGLG.padding.left));
        }
        var RewardGLG = RewardContent.GetComponent<GridLayoutGroup>();
        RewardContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, RewardContent.sizeDelta.x + (((RewardGLG.cellSize.x + RewardGLG.spacing.x) * RewardContent.childCount) + RewardGLG.padding.left));
    }
    public void AddRewards(RewardInformation.Reward_Information clear, int type = 0) // type 1 : first clear, type 2 : 3 star clear
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
            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.SetParent(RewardContent, false);
            Image image = obj.GetComponentsInChildren<Image>()[1];
            image.sprite = jem_sprite;
            image.SetNativeSize();
            image.GetComponent<RectTransform>().sizeDelta /= image_devide_value;

            obj.GetComponentInChildren<TextMeshProUGUI>().text = $"{clear.jem}";

            rt.GetChild(type + 1).gameObject.SetActive(true);
        }

        if (clear.coin > 0)
        {
            obj = Instantiate(ClearPrefab);
            obj.name = "Coin";
            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.SetParent(RewardContent, false);
            Image image = obj.GetComponentsInChildren<Image>()[1];
            image.sprite = coin_sprite;
            image.SetNativeSize();
            image.GetComponent<RectTransform>().sizeDelta /= image_devide_value;

            obj.GetComponentInChildren<TextMeshProUGUI>().text = $"{clear.coin}";

            rt.GetChild(type + 1).gameObject.SetActive(true);
        }
    }

    public void AddWaveDatas()
    {
        var sprites = StageManager.Instance.GetEnemiesSprite();
        GameObject obj = null;
        RectTransform rt;
        Image image;

        foreach (var sprite in sprites)
        {
            obj = Instantiate(ClearPrefab);
            rt = obj.GetComponent<RectTransform>();
            rt.SetParent(MobContent, false);

            image = obj.GetComponentsInChildren<Image>()[1];
            image.sprite = sprite;
            image.SetNativeSize();
            image.GetComponent<RectTransform>().sizeDelta /= image_devide_value;
        }
    }
}
