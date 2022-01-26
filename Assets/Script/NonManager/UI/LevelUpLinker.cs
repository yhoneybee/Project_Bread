using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpLinker : MonoBehaviour
{
    public RectTransform rtrnRewardParent;
    public RectTransform rtrnRealParent;
    public TextMeshProUGUI txtCoinCount;
    public TextMeshProUGUI txtJemCount;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rtrnRealParent.gameObject.SetActive(false);
        }
    }
}
