using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Collection : MonoBehaviour
{
    public enum CollectionType
    {
        Bread,
        Item,
        Enemy
    }

    [Header("대입 필요 X")]
    [SerializeField] Image image;
    [Header("카드 좌측 상단 등급 이미지")]
    [SerializeField] Image rank_image;
    [Header("카드 우측 상단 등급 텍스트")]
    [SerializeField] TextMeshProUGUI text;
    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {

    }

    /// <summary>
    /// Collection의 이미지, 텍스트 등을 설정해주는 함수
    /// </summary>
    /// <param name="linker">현재 씬의 Collection Linker</param>
    /// <param name="unit">해당 Collection에 맞는 Unit</param>
    public void SetCollection(CollectionLinker linker, Unit unit)
    {
        image.sprite = unit.Info.Icon;
        image.SetNativeSize();

        float devide_value = 1.1f;
        while (true)
        {
            image.rectTransform.sizeDelta /= devide_value;

            if (image.rectTransform.sizeDelta.x < 220 && image.rectTransform.sizeDelta.y < 150)
                break;
            else
                image.rectTransform.sizeDelta *= devide_value;

            devide_value += 0.1f;
        }

        Rank rank = unit.Info.Rank;
        text.text = rank.ToString();

        Sprite rank_sprite = new Sprite[4] { linker.common_icon, linker.rare_icon, linker.epic_icon, linker.legend_icon }[(int)(rank)];

        rank_image.sprite = rank_sprite;
        rank_image.SetNativeSize();
        rank_image.rectTransform.sizeDelta /= 10;
    }

    /// <summary>
    /// Collection의 이미지, 텍스트 등을 설정해주는 함수
    /// </summary>
    /// <param name="linker">현재 씬의 Collection Linker</param>
    /// <param name="item">해당 Collection에 맞는 Item</param>
    public void SetCollection(CollectionLinker linker, Item item)
    {
        image.sprite = item.Icon;
        image.SetNativeSize();
        image.rectTransform.sizeDelta /= 2;
    }
}
