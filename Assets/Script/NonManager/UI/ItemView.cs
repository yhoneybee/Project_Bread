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
                Img.sprite = item.Icon;
            }
            else
                Img.color = Color.clear;
        }
    }
    public Image Img;
    public Button Btn;
}
