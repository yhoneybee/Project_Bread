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
        SetRewards();
    }
    void Update()
    {
    }
    void SetRewards()
    {
        System.Tuple<int, int, int> reward;

        for (int i = 0; i < level_rewards.Length; i++)
        {
            reward = reward_list[i];
            // AddListener ÀÎµ¦½º ¹ö±× °íÃÄ¾ßµÊ; ÁøÂ¥ °Ì³ª•¾Ä¡³é
            level_rewards[i].button.onClick.AddListener(() => { TakeReward(reward.Item1, reward.Item2, reward.Item3, i); });
        }
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
        LevelReward reward;
        for (int i = 0; i < level_rewards.Length; i++)
        {
            reward = level_rewards[i];

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
    void TakeReward(int coin, int jem, int stemina, int reward_index)
    {
        GameManager.Instance.Coin += coin;
        GameManager.Instance.Jem += jem;
        GameManager.Instance.Stemina += stemina;

        level_reward_data.reward_case[reward_index] = RewardCase.IS_TAKED;

        SetInterface();
    }
}