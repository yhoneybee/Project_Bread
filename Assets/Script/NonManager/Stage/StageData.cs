using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "StageData", menuName = "Datas/StageData")]
public class StageData : ScriptableObject
{
    public List<WaveInformation> wave_information;
    public UnityEngine.Sprite[] enemies_sprite;
    [Space(15)]
    public RewardInformation reward_information;
    public UnityEngine.Sprite[] rewards_sprite;
    [Space(15)]
    public int star_count = 0;
    public bool is_startable;
    [Space(15)]
    public float tower_hp;
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

    public Reward_Information first_clear;
    public Reward_Information three_star_clear;
    public Reward_Information basic_clear;
}