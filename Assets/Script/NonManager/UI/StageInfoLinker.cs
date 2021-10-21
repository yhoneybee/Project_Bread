using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StageInfoLinker : MonoBehaviour
{
    public enum Reward_Kind
    {
        Coin,
        Jem,
        Unit
    }

    [SerializeField] GameObject ClearPrefab;

    [SerializeField] RectTransform MobContent;
    [SerializeField] RectTransform RewardContent;

    [SerializeField] Sprite coin_sprite = null;
    [SerializeField] Sprite jem_sprite = null;
    [SerializeField] List<Sprite> Nums = new List<Sprite>();
    [SerializeField] List<Sprite> ThemeNames = new List<Sprite>();

    [SerializeField] List<Image> Stars = new List<Image>();
    [SerializeField] Image Where;
    [SerializeField] Image Theme;
    [SerializeField] Image StageTen;
    [SerializeField] Image StageOne;

    [SerializeField] SwitchSprite StarSpirtes;

    [SerializeField] float image_devide_value;

    bool is_played = false;

    private void Start()
    {
        SetRewards();

        if (ButtonActions.Instance.CheckReEntering("E - 01 DeckView"))
        {
            var stage_data = StageManager.Instance.GetStage(StageInfo.stage_number - 1);

            for (int i = 0; i < 3; i++)
            {
                if (stage_data.star_count > i) Stars[i].sprite = StarSpirtes.ASprite;
                else Stars[i].sprite = StarSpirtes.BSprite;
            }

            Where.sprite = ThemeNames[StageInfo.theme_number - 1];

            Theme.sprite = Nums[StageInfo.theme_number - 1];

            int dev = StageInfo.stage_number / 10;
            StageTen.sprite = Nums[dev == 0 ? 9 : dev - 1];
            int mod = StageInfo.stage_number % 10;
            StageOne.sprite = Nums[mod == 0 ? 9 : mod - 1];
        }
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
        if (ButtonActions.Instance.CheckReEntering("E - 01 DeckView"))
            RewardContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, RewardContent.sizeDelta.y + (((RewardGLG.cellSize.y + RewardGLG.spacing.y) * (RewardContent.childCount / 6)) + RewardGLG.padding.top));
        else
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
            obj = Instantiate(ClearPrefab, RewardContent, false);
            obj.name = "Jem";
            RectTransform rt = obj.GetComponent<RectTransform>();
            Image image = obj.GetComponentsInChildren<Image>()[1];
            image.sprite = jem_sprite;
            image.SetNativeSize();
            var childRTf = image.GetComponent<RectTransform>();
            childRTf.sizeDelta /= image_devide_value;
            childRTf.pivot = Vector2.one * 0.5f;
            childRTf.anchorMax = Vector2.one * 0.5f;
            childRTf.anchorMin = Vector2.one * 0.5f;

            obj.GetComponentInChildren<TextMeshProUGUI>().text = $"{clear.jem}";

            rt.GetChild(type + 1).gameObject.SetActive(true);
        }

        if (clear.coin > 0)
        {
            obj = Instantiate(ClearPrefab, RewardContent, false);
            obj.name = "Coin";
            RectTransform rt = obj.GetComponent<RectTransform>();
            Image image = obj.GetComponentsInChildren<Image>()[1];
            image.sprite = coin_sprite;
            image.SetNativeSize();
            var childRTf = image.GetComponent<RectTransform>();
            childRTf.sizeDelta /= image_devide_value;
            childRTf.pivot = Vector2.one * 0.5f;
            childRTf.anchorMax = Vector2.one * 0.5f;
            childRTf.anchorMin = Vector2.one * 0.5f;

            obj.GetComponentInChildren<TextMeshProUGUI>().text = $"{clear.coin}";

            rt.GetChild(type + 1).gameObject.SetActive(true);
        }
    }
    public (int, int, Unit) GetRewards(bool first, bool three_star)
    {
        int coin = 0, jem = 0; Unit unit = null;

        RewardInformation rewards = StageManager.Instance.GetStage().reward_information;

        if (first)
        {
            coin += rewards.first_clear.coin;
            jem += rewards.first_clear.jem;
        }

        if (three_star)
        {
            coin += rewards.three_star_clear.coin;
            jem += rewards.three_star_clear.jem;
        }

        coin += rewards.basic_clear.coin;
        jem += rewards.three_star_clear.jem;

        return (coin, jem, unit);
    }

    public void AddWaveDatas()
    {
        var sprites = StageManager.Instance.GetEnemiesSprite();
        GameObject obj = null;
        Image image;

        foreach (var sprite in sprites)
        {
            obj = Instantiate(ClearPrefab, MobContent, false);

            image = obj.GetComponentsInChildren<Image>()[1];
            image.sprite = sprite;
            image.SetNativeSize();
            var childRTf = image.GetComponent<RectTransform>();
            childRTf.sizeDelta /= image_devide_value;
            childRTf.pivot = Vector2.one * 0.5f;
            childRTf.anchorMax = Vector2.one * 0.5f;
            childRTf.anchorMin = Vector2.one * 0.5f;
        }
    }
}
