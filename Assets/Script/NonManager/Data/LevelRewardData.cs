using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelRewardData", menuName = "Datas/LevelRewardData")]
public class LevelRewardData : ScriptableObject
{
    public RewardCase[] reward_case;
}

