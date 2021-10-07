using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RewardInfo
{
    public Tuple<int, int, Unit> FirstClear;
    public Tuple<int, int, Unit> ThreeStarClear;
    public Tuple<int, int, Unit> Clear;
}

[Serializable]
public struct  Wave_Data
{
    public List<WaveData> stage_waves;
}

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; } = null;

    public List<RewardInfo>[] RewardInfos;

    public Wave_Data[] theme_waves;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        RewardInfos = new List<RewardInfo>[5]
        {
            new List<RewardInfo>()
            {
                new RewardInfo{ Clear = new Tuple<int, int, Unit>(1,1,null), FirstClear = new Tuple<int, int, Unit>(10,10,null), ThreeStarClear = new Tuple<int, int, Unit>(100,100,null)},
                new RewardInfo{ Clear = new Tuple<int, int, Unit>(2,2,null), FirstClear = new Tuple<int, int, Unit>(20,20,null), ThreeStarClear = new Tuple<int, int, Unit>(200,200,null)},
                new RewardInfo{ Clear = new Tuple<int, int, Unit>(3,3,null), FirstClear = new Tuple<int, int, Unit>(30,30,null), ThreeStarClear = new Tuple<int, int, Unit>(300,300,null)},
                new RewardInfo{ Clear = new Tuple<int, int, Unit>(4,4,null), FirstClear = new Tuple<int, int, Unit>(40,40,null), ThreeStarClear = new Tuple<int, int, Unit>(400,400,null)},
                new RewardInfo{ Clear = new Tuple<int, int, Unit>(5,5,null), FirstClear = new Tuple<int, int, Unit>(50,50,null), ThreeStarClear = new Tuple<int, int, Unit>(500,500,null)},
                new RewardInfo{ Clear = new Tuple<int, int, Unit>(6,6,null), FirstClear = new Tuple<int, int, Unit>(60,60,null), ThreeStarClear = new Tuple<int, int, Unit>(600,600,null)},
            },
            new List<RewardInfo>()
            {

            },
            new List<RewardInfo>()
            {

            },
            new List<RewardInfo>()
            {

            },
            new List<RewardInfo>()
            {

            },
        };

        for (int i = 0; i < 5; i++)
        {
            var rewards = RewardInfos[i];

            SaveManager.Save(rewards, $"{i}_Theme_Reward");
        }
    }

    public RewardInfo GetReward() => RewardInfos[StageInfo.theme_number - 1][StageInfo.stage_number - 1];
    public WaveData GetWaveData() => theme_waves[StageInfo.theme_number].stage_waves[StageInfo.stage_number - 1];
}
