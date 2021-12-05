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

        }
    }
}
