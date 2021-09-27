using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitView : MonoBehaviour
{
    private Unit show;
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
            if (UILinker.Icon) UILinker.Icon.sprite = show.Info.Icon;

            if (UILinker.LevelText)
            {
                UILinker.LevelText.text = $"Lv.{show.Info.Level}";
                UILinker.NameText.text = $"{show.Info.Name}";
            }
            else UILinker.NameText.text = $"Lv.{show.Info.Level} {show.Info.Name}";
        }
        else
        {
            if (UILinker.Icon) UILinker.Icon.sprite = UIManager.Instance.UnitNullSprite;
            if (UILinker.NullUnActive) UILinker.NullUnActive.SetActive(false);
        }
    }
}
