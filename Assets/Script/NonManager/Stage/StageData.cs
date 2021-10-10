using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "StageData", menuName = "Datas/StageData")]
public class StageData : ScriptableObject
{
    public WaveInformation wave_information;
    public Sprite[] enemies_sprite;
    [Space(15)]
    public RewardInformation reward_information;
    public Sprite[] rewards_sprite;
    [Space(15)]
    public int star_count = 0; // 획득한 별 개수
    public int three_star_count = 0; // 별 3개 제한 시간
    public bool is_startable;
}

[Serializable]
public struct WaveInformation
{
    [Serializable]
    public struct Wave_Information
    {
        public Unit unit;
        public float delay;
    }
    public Wave_Information[] wave_information;
}

[Serializable]
public struct RewardInformation
{
    [Serializable]
    public struct Reward_Information
    {
        public int coin;
        public int jem;
        public Unit unit;
    }

    // 처음으로 클리어했을 때 보상
    public Reward_Information first_clear;
    // 3개 별 획득하였을 때 보상
    public Reward_Information three_star_clear;
    // 기본 클리어 보상
    public Reward_Information basic_clear;
}