using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RewardCase
{
    TAKEABLE,
    IS_TAKED,
    NONE,
}
public class LevelRewardLinker : MonoBehaviour
{
    [SerializeField] LevelRewardData level_reward_data;
    [System.Serializable]
    class LevelReward
    {
        public Button button;
        public Image button_image;
        public int reward_index;
    }

    [SerializeField] Sprite take_reward_sprite;
    [SerializeField] Sprite already_taked_reward_sprite;

    [SerializeField] Sprite coin_sprite;
    [SerializeField] Sprite jem_sprite;
    [SerializeField] Sprite stemina_sprite;

    [SerializeField] LevelReward[] level_rewards;

    // Coin, Jem, Stemina
    List<System.Tuple<int, int, int>> reward_list = new List<System.Tuple<int, int, int>>()
        {
            new System.Tuple<int, int, int>(1, 1, 1),
            new System.Tuple<int, int, int>(1, 1, 1),
            new System.Tuple<int, int, int>(1, 1, 1),
            new System.Tuple<int, int, int>(1, 1, 1),
            new System.Tuple<int, int, int>(1, 1, 1),
            new System.Tuple<int, int, int>(1, 1, 1),
            new System.Tuple<int, int, int>(1, 1, 1),
            new System.Tuple<int, int, int>(1, 1, 1),
            new System.Tuple<int, int, int>(1, 1, 1),
            new System.Tuple<int, int, int>(1, 1, 1)
        };
    void OnEnable()
    {
        CheckRewardsInfo();
        SetInterface();
    }
    private void Start()
    {
    }
    void Update()
    {
    }
    void CheckRewardsInfo()
    {
        for (int i = 1; i <= 10; i++)
        {
            if (GameManager.Instance.player_level >= i * 10 && level_reward_data.reward_case[i - 1] == RewardCase.NONE)
            {
                level_reward_data.reward_case[i - 1] = RewardCase.TAKEABLE;
            }
            else if (GameManager.Instance.player_level < i * 10)
            {
                level_reward_data.reward_case[i - 1] = RewardCase.NONE;
            }
        }
    }
    void SetInterface()
    {
        GridLayoutGroup gg;
        LevelReward reward;

        for (int i = 0; i < level_rewards.Length; i++)
        {
            gg = GetComponentsInChildren<GridLayoutGroup>()[i + 1];
            reward = level_rewards[i];

            gg.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = coin_sprite;
            gg.transform.GetChild(0).gameObject.SetActive(reward_list[i].Item1 != 0);
            gg.transform.GetChild(0).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = reward_list[i].Item1.ToString();

            gg.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = jem_sprite;
            gg.transform.GetChild(1).gameObject.SetActive(reward_list[i].Item2 != 0);
            gg.transform.GetChild(1).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = reward_list[i].Item2.ToString();

            gg.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = stemina_sprite;
            gg.transform.GetChild(2).gameObject.SetActive(reward_list[i].Item3 != 0);
            gg.transform.GetChild(2).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = reward_list[i].Item3.ToString();


            switch (level_reward_data.reward_case[i])
            {
                case RewardCase.TAKEABLE:
                    reward.button_image.sprite = take_reward_sprite;
                    reward.button_image.color = Color.white;
                    reward.button.enabled = true;
                    break;
                case RewardCase.IS_TAKED:
                    reward.button_image.sprite = already_taked_reward_sprite;
                    reward.button_image.color = Color.white;
                    reward.button.enabled = false;
                    break;
                case RewardCase.NONE:
                    reward.button_image.sprite = take_reward_sprite;
                    reward.button_image.color = new Color(0.5f, 0.5f, 0.5f);
                    reward.button.enabled = false;
                    break;
            }
        }
    }

    public void TakeReward(int reward_index)
    {
        GameManager.Instance.Coin += reward_list[reward_index].Item1;
        GameManager.Instance.Jem += reward_list[reward_index].Item2;
        GameManager.Instance.Stemina += reward_list[reward_index].Item3;

        level_reward_data.reward_case[reward_index] = RewardCase.IS_TAKED;

        SetInterface();
    }
}