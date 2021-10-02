using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Datas/WaveData", order = 0)]
public class WaveData : ScriptableObject
{
    [Serializable]
    public struct Wave_Information
    {
        public Unit unit;
        public float delay;
    }
    public Wave_Information[] wave_information;

}
