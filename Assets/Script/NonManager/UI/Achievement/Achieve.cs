using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Achieve : MonoBehaviour
{
    [SerializeField] int achieve_id;
    [SerializeField] Button take_button;
    public enum AchieveCondition
    {
        None,
        Takable,
        Taked
    }
    public AchieveCondition achieve_condition;
    public abstract bool IsTakable();
    protected virtual void Start()
    {
        achieve_condition = (AchieveCondition)PlayerPrefs.GetInt(achieve_id.ToString(), (int)AchieveCondition.None);

        if (IsTakable() && achieve_condition != AchieveCondition.Taked)
        {
            achieve_condition = AchieveCondition.Takable;
            take_button.onClick.AddListener(PressButton);
            PlayerPrefs.SetInt(achieve_id.ToString(), (int)AchieveCondition.Takable);
        }
    }

    public virtual void PressButton()
    {
        PlayerPrefs.SetInt(achieve_id.ToString(), (int)AchieveCondition.Taked);
    }
}