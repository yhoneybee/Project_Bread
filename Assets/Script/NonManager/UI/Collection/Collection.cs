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

    [Header("���� �ʿ� X")]
    [SerializeField] Image image;
    [Header("ī�� ���� ��� ��� �̹���")]
    [SerializeField] Image rank_image;
    [Header("ī�� ���� ��� ��� �ؽ�Ʈ")]
    [SerializeField] TextMeshProUGUI text;
    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {

    }

    /// <summary>
    /// Collection�� �̹���, �ؽ�Ʈ ���� �������ִ� �Լ�
    /// </summary>
    /// <param name="linker">���� ���� Collection Linker</param>
    /// <param name="unit">�ش� Collection�� �´� Unit</param>
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
    /// Collection�� �̹���, �ؽ�Ʈ ���� �������ִ� �Լ�
    /// </summary>
    /// <param name="linker">���� ���� Collection Linker</param>
    /// <param name="item">�ش� Collection�� �´� Item</param>
    public void SetCollection(CollectionLinker linker, Item item)
    {
        image.sprite = item.Icon;
        image.SetNativeSize();
        image.rectTransform.sizeDelta /= 2;
    }
}
