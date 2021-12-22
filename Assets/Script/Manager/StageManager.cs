using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Stage_Data
{
    public List<StageData> stages;
}

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; } = null;

    public bool theme_clear { get; set; } = false;
    public Stage_Data[] all_themes;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        RewardInformation[] rewards = new RewardInformation[10];

        for (int theme = 0; theme < GameManager.Instance.theme_count; theme++)
        {
            for (int stage = 0; stage < 10; stage++)
            {
                rewards[stage] = all_themes[theme].stages[stage].reward_information;
            }

            GameManager.Instance.onAutoSave += () => { SaveManager.Save(rewards, $"{theme}_Theme_Reward"); };
        }
    }

    public StageData GetStage(int theme, int stage) => all_themes[theme].stages[stage];
    public StageData GetStage(int stage) => GetStage(StageInfo.theme_number - 1, stage);
    public StageData GetStage() => GetStage(StageInfo.stage_number - 1);
    public List<WaveInformation> GetWaveData() => GetStage().wave_information;
    public RewardInformation GetReward() => GetStage().reward_information;
    public Sprite[] GetEnemiesSprite() => GetStage().enemies_sprite;
    public Sprite[] GetRewardsSprite() => GetStage().rewards_sprite;
}
