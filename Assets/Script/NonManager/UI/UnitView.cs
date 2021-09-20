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
            UILinker.NullUnActive.SetActive(true);

            UILinker.Icon.sprite = show.Info.Icon;
            if (UILinker.LevelText)
            {
                UILinker.LevelText.text = $"Lv.{show.Info.Level}";
                UILinker.NameText.text = $"{show.Info.Name}";
            }
            else UILinker.NameText.text = $"Lv.{show.Info.Level} {show.Info.Name}";
        }
        else
        {
            UILinker.NullUnActive.SetActive(false);
        }
    }
}
