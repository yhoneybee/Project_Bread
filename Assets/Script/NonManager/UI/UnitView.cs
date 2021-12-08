using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitView : MonoBehaviour
{
    [SerializeField] private Unit show;
    public Unit Show
    {
        get { return show; }
        set
        {
            show = value;
            SetLinker(show);
        }
    }

    public UILinker UILinker;

    public void SetLinker(Unit show)
    {
        if (show)
        {
            if (UILinker.NullUnActive) UILinker.NullUnActive.SetActive(true);
            if (UILinker.Icon)
            {
                UILinker.Icon.color = Color.white;
                UILinker.Icon.sprite = show.Info.Icon;

                if (ButtonActions.Instance.CheckReEntering("E-01_DeckView"))
                    UIManager.Instance.FixSizeToRatio(UILinker.Icon, GetComponent<RectTransform>().sizeDelta.x - 20);
                else
                    UIManager.Instance.FixSizeToRatio(UILinker.Icon, GetComponent<RectTransform>().sizeDelta.x - 100);
            }
            if (UILinker.LevelText)
            {
                UILinker.LevelText.text = $"Lv.{show.Info.Level}";
                UILinker.NameText.text = $"{show.Info.Name}";
            }
            else if (UILinker.NameText) UILinker.NameText.text = $"Lv.{show.Info.Level} {show.Info.Name}";
            if (UILinker.imgRank)
            {
                UILinker.imgRank.gameObject.SetActive(true);
                UILinker.imgRank.sprite = UIManager.Instance.spRanks[((int)show.Info.Rank)];
                var rtrnImgRank = UILinker.imgRank.GetComponent<RectTransform>();
                UIManager.Instance.FixSizeToRatio(UILinker.imgRank, 125);
                rtrnImgRank.anchorMin = Vector2.one;
                rtrnImgRank.anchorMax = Vector2.one;
                rtrnImgRank.pivot = Vector2.one;
            }
            if (UILinker.imgRankBg)
            {
                UILinker.imgRankBg.gameObject.SetActive(true);
                UILinker.imgRankBg.sprite = UIManager.Instance.spRankBgs[((int)show.Info.Rank)];
            }
        }
        else
        {
            if (UILinker.Icon)
            {
                if (ButtonActions.Instance.CheckReEntering("E-01_DeckView"))
                    UILinker.Icon.color = Color.clear;
                else
                    UILinker.Icon.GetComponent<RectTransform>().sizeDelta = UILinker.IconRestore.sizeDelta;
                UILinker.Icon.sprite = UIManager.Instance.UnitNullSprite;
            }
            if (UILinker.NullUnActive) UILinker.NullUnActive.SetActive(false);
            if (UILinker.imgRank)
            {
                UILinker.imgRank.gameObject.SetActive(false);
            }
            if (UILinker.imgRankBg)
            {
                UILinker.imgRankBg.gameObject.SetActive(false);
            }
        }
    }
}
