using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionLinker : MonoBehaviour
{
    [SerializeField] GameObject collection_prefab;

    [SerializeField] RectTransform bread_collection;
    [SerializeField] RectTransform item_collection;
    [SerializeField] RectTransform enemy_collection;

    //[SerializeField]
    public Unit[] got_breads;
    //[SerializeField]
    public Item[] got_items;
    //[SerializeField]
    public Unit[] got_enemys;

    public Sprite common_icon;
    public Sprite rare_icon;
    public Sprite epic_icon;
    public Sprite legend_icon;

    RectTransform bread_content;
    RectTransform item_content;
    RectTransform enemy_content;

    GameObject current_showing_collection;

    void GetContents()
    {
        RectTransform content = (RectTransform)(bread_collection.GetChild(0).GetChild(0).transform);
        bread_content = content;

        content = (RectTransform)(item_collection.GetChild(0).GetChild(0).transform);
        item_content = content;

        content = (RectTransform)(enemy_collection.GetChild(0).GetChild(0).transform);
        enemy_content = content;

    }

    /// <summary>
    /// �� Content�� Grid layout group ũ�⸦ �������ִ� �Լ�
    /// </summary>
    /// <param name="content_index">0 : bread content, 1 : item_content, 2 : enemy content</param>
    void SetContentsSize(int content_index)
    {
        // �ε��� ������ ��� ��� ��ȯ �� ���� ���
        if (content_index > 2)
        {
            Debug.LogError($"{nameof(content_index)} is out of range ({nameof(CollectionLinker)}.cs)");
            return;
        }

        // �� ������ collection contents
        RectTransform[] target_contents = new RectTransform[3] { bread_content, item_content, enemy_content };//[content_index];
        RectTransform target_content = target_contents[content_index];

        // ���� ���� �� ������ collections
        Object[][] target_objects = new Object[3][] { got_breads, got_items, got_enemys };//[content_index];
        Object[] target_object = target_objects[content_index];

        // target_content�� Grid layout group ������Ʈ�� ���� �ֱ� ������ collections�� ������ �°� ũ�� ����
        target_content.sizeDelta = new Vector2(target_content.sizeDelta.x, (target_object.Length / 4) * (300 + 10 + 20)); // 300 : cell size, 10 : spacing size, 20 : padding size

        // ��� ������ collection�� ��ȯ�ϸ� ������ ����
        for (int i = 0; i < target_object.Length; i++)
        {
            // ���� �� �ٷ� �ش� collection ����
            Instantiate(collection_prefab, target_content);
            Transform t_collection = target_content.GetChild(i);
            Collection collection = t_collection.GetComponent<Collection>();

            // collection�� ������ Item�� �ƴ� ��� (���� ��ȯ�� ���� �˻�)
            if (content_index != 1)
                collection.SetCollection(this, (Unit)(target_object[i]));
            else
                collection.SetCollection(this, (Item)(target_object[i]));
        }
    }

    void Start()
    {
        GetContents();

        for (int i = 0; i < 3; i++)
            SetContentsSize(i);

        current_showing_collection = bread_collection.gameObject;
    }

    void Update()
    {
    }

    /// <summary>
    /// �Ű������� �Ѱܹ��� index�� ���� �ش� index�� �´� ������ Collection�� Ȱ��ȭ
    /// </summary>
    /// <param name="content_index"></param>
    public void ShowCollection(int content_index)
    {
        GameObject target_collection = new RectTransform[3] { bread_collection, item_collection, enemy_collection }[content_index].gameObject;

        current_showing_collection.SetActive(false);
        target_collection.SetActive(true);

        current_showing_collection = target_collection;
    }
}
