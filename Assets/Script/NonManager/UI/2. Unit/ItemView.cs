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

    private void FixedUpdate()
    {
        imgOwner.gameObject.SetActive(true);
        if (item.Owner)
        {
            imgOwner.sprite = item.Owner.Info.Icon;
            UIManager.Instance.FixSizeToRatio(imgOwner, 70, 1, 1, 1, 1);
        }
        else imgOwner.gameObject.SetActive(false);
    }
    public Image Img;
    public Image imgOwner;
    public Button Btn;
}
