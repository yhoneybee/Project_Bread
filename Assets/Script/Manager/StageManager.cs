using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RewardInfo
{
    public Tuple<int, int, Unit> FirstClear;
    public Tuple<int, int, Unit> ThreeStarClear;
}

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; } = null;

    public List<RewardInfo> RewardInfos;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {

    }
}
