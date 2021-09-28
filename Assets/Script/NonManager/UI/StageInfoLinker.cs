using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageInfoLinker : MonoBehaviour
{
    public RectTransform MobContent;
    public RectTransform RewardContent;

    private void Start()
    {
        RewardInfo reward = StageManager.Instance.GetReward();
    }
}
