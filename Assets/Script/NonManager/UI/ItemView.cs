using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemView : MonoBehaviour
{
    private Item item;
    public Item Item
    {
        get { return item; }
        set
        {
            item = value;
            if (item && item.Icon)
            {
                Img.color = Color.white;
                Btn.enabled = true;
                if (!item.gotten)
                {
                    Img.color = Color.grey;
                    Btn.enabled = false;
                }
                Img.sprite = item.Icon;
            }
            else
                Img.color = Color.clear;

            UIManager.Instance.FixSizeToRatio(Img, ItemManager.Instance.ItemContent.GetComponent<GridLayoutGroup>().cellSize.x);
        }
    }
    public Image Img;
    public Button Btn;
}
