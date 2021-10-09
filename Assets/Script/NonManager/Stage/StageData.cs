using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "StageData", menuName = "Datas/StageData", order = 1)]
public class StageData : ScriptableObject
{
    public WaveInformation wave_information;
    public RewardInformation reward_information;
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
    public Reward_Information FirstClear;
    public Reward_Information ThreeStarClear;
    public Reward_Information Clear;
}