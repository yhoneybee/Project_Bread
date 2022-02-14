using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardInfoLinker : MonoBehaviour
{
    public int Count
    {
        get => count;
        set
        {
            count = value;
            txtCount.text = $"{count:#,0}";
        }
    }
    private int count;
    public ProductType Kind
    {
        get => kind;
        set
        {
            kind = value;
            OnEnable();
        }
    }
    private ProductType kind;
    public TextMeshProUGUI txtCount;
    public Image img;

    void OnEnable()
    {
        img.sprite = Kind switch
        {
            ProductType.COIN => UIManager.Instance.IconSprites[0],
            ProductType.JEM => UIManager.Instance.IconSprites[1],
            ProductType.STEMINA => UIManager.Instance.IconSprites[3],
            _ => null,
        };
        UIManager.Instance.FixSizeToRatio(img, 100);
    }

    public void Show(ProductType productType, int count)
    {
        Kind = productType;
        Count = count;
        transform.parent.gameObject.SetActive(true);
    }
}
