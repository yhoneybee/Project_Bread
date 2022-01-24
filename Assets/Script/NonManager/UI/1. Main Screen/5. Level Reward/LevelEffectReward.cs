using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelEffectReward : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> txtEffectReward;

    private void OnEnable()
    {
        for (int i = 1; i <= GameManager.Instance.PlayerLevel / 10; i++)
            txtEffectReward[i - 1].color = Color.white;
    }
}
