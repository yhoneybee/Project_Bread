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
    public int star_count = 0; // ȹ���� �� ����
    public int three_star_count = 0; // �� 3�� ���� �ð�
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

    // ó������ Ŭ�������� �� ����
    public Reward_Information first_clear;
    // 3�� �� ȹ���Ͽ��� �� ����
    public Reward_Information three_star_clear;
    // �⺻ Ŭ���� ����
    public Reward_Information basic_clear;
}