using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StageManager : MonoBehaviour
{
    List<Tuple<int, int, Unit>>[] Rewards;

    Tuple<int, int, Unit> Reword => Rewards[StageInfo.theme_number][StageInfo.stage_number];

    private void Start()
    {
        Rewards = new List<Tuple<int, int, Unit>>[5];
        Rewards[0] = new List<Tuple<int, int, Unit>>()
        {

        };
        Rewards[1] = new List<Tuple<int, int, Unit>>()
        {

        };
        Rewards[2] = new List<Tuple<int, int, Unit>>()
        {

        };
        Rewards[3] = new List<Tuple<int, int, Unit>>()
        {

        };
        Rewards[4] = new List<Tuple<int, int, Unit>>()
        {

        };
    }

    public (int jem, int money, Unit unit) GetReward() => (Reword.Item1, Reword.Item2, Reword.Item3);
}
