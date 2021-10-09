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
            SaveManager.Save(rewards, $"{theme}_Theme_Reward");

        }
    }

    public StageData GetStage(int index) => all_themes[StageInfo.theme_number - 1].stages[index];
    public StageData GetStage() => GetStage(StageInfo.stage_number - 1);
    public WaveInformation GetWaveData() => GetStage().wave_information;
    public RewardInformation GetReward() => GetStage().reward_information;
}
