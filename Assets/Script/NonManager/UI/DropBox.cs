using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBox : MonoBehaviour
{
    void FixedUpdate()
    {
        if (!ShopManager.Instance.BuyWindow.gameObject.activeSelf)
        {
            ShopManager.Instance.imgDropBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(-600, 1000);
        }
    }
}
