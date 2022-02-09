using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvents : MonoBehaviour
{
    public void ReturnObj()
    {
        IngameManager.Instance.ReturnDamageText(GetComponent<RectTransform>());
    }
}
