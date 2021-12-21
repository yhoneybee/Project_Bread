using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[CreateAssetMenu(fileName = "Achieve", menuName = "Datas/Achieve", order = 1)]
public abstract class Achieve : MonoBehaviour
{
    [SerializeField] int achieve_id;
    [SerializeField] Button take_button;
    [SerializeField] Image reward_image;
    [SerializeField] TMPro.TextMeshProUGUI reward_text;

    public int id => achieve_id;
    public enum AchieveType
    {
        Adventure,
        Content,
        System,
        Character
    }
    public AchieveType achieve_type;
    public enum AchieveCondition
    {
        None,
        Takable,
        Taked
    }
    public AchieveCondition achieve_condition;
    public abstract bool IsTakable();
    public void Setting()
    {
        take_button = transform.GetComponentInChildren<Button>();
        reward_image = take_button.transform.GetComponentsInChildren<Image>()[1];
        reward_text = take_button.transform.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        achieve_condition = (AchieveCondition)PlayerPrefs.GetInt(achieve_id.ToString(), (int)AchieveCondition.None);

        if (IsTakable() && achieve_condition != AchieveCondition.Taked)
        {
            achieve_condition = AchieveCondition.Takable;
            take_button.onClick.AddListener(PressButton);
            PlayerPrefs.SetInt(achieve_id.ToString(), (int)AchieveCondition.Takable);
        }
        else if (achieve_condition == AchieveCondition.Taked)
            take_button.image.color = reward_image.color = reward_text.color = new Color(0.5f, 0.5f, 0.5f);
    }

    public virtual void PressButton()
    {
        PlayerPrefs.SetInt(achieve_id.ToString(), (int)AchieveCondition.Taked);
        take_button.image.color = reward_image.color = reward_text.color = new Color(0.5f, 0.5f, 0.5f);
    }
}