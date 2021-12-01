using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

// 대호 개발, Collection Scene의 UI를 총괄하는 클래스
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

    [SerializeField] TextMeshProUGUI percent_text;
    [SerializeField] Slider percent_slider;

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
    /// 각 Content의 Grid layout group 크기를 설정해주는 함수
    /// </summary>
    /// <param name="content_index">0 : bread content, 1 : item_content, 2 : enemy content</param>
    void SetContentsSize(int content_index)
    {
        // 인덱스 범위를 벗어날 경우 반환 및 오류 출력
        if (content_index > 2)
        {
            Debug.LogError($"{nameof(content_index)} is out of range ({nameof(CollectionLinker)}.cs)");
            return;
        }

        // 세 종류의 collection contents
        RectTransform[] target_contents = new RectTransform[3] { bread_content, item_content, enemy_content };//[content_index];
        RectTransform target_content = target_contents[content_index];

        // Unit으로 형변환하여 사용되는, 보유 중인지 확인하기 위한 세 종류의 배열
        Object[][] target_objects = new Object[3][] { got_breads, got_items, got_enemys };//[content_index];
        Object[] target_object = target_objects[content_index];

        // 띄울 모든 Unit (빵 or 적군)
        var all_units = from unit in UnitManager.Instance.Units
                        group unit by unit.UnitType == UnitType.FRIEND into my_group
                        select new { is_bread = my_group.Key, units = my_group };

        List<Unit> breads = new List<Unit>();
        List<Unit> enemies = new List<Unit>();

        foreach (var unit_kind in all_units)
        {
            foreach (var unit in unit_kind.units)
            {
                if (unit_kind.is_bread)
                {
                    breads.Add(unit);
                }
                else
                {
                    enemies.Add(unit);
                }
            }
        }

        Object[][] show_objects = new Object[3][] { breads.ToArray(), GameManager.Instance.Items.ToArray(), enemies.ToArray() };
        Object[] show_object = show_objects[content_index];

        // target_content는 Grid layout group 컴포넌트를 갖고 있기 때문에 collections의 개수에 맞게 크기 조절
        target_content.sizeDelta = new Vector2(target_content.sizeDelta.x, (show_object.Length / 4) * (300 + 10 + 20)); // 300 : cell size, 10 : spacing size, 20 : padding size
        if (show_object.Length % 4 > 0)
            target_content.sizeDelta += new Vector2(0, 300 + 10 + 20);

        // 모든 종류의 collection을 순환하며 정보를 설정
        for (int i = 0; i < show_object.Length; i++)
        {
            // 생성 후 바로 해당 collection 설정
            Instantiate(collection_prefab, target_content);
            Transform t_collection = target_content.GetChild(i);
            Collection collection = t_collection.GetComponent<Collection>();

            bool is_got;
            // collection의 종류가 Item이 아닐 경우 (형식 변환을 위해 검사)
            if (content_index != 1)
            {
                is_got = target_object.ToList<Object>().Find((o) => { return ((Unit)o).Info.Name == ((Unit)(show_object[i])).Info.Name; }) != null;
                collection.SetCollection(this, (Unit)(show_object[i]), is_got);
            }
            else
            {
                is_got = GameManager.Instance.Items.Find((o) => { return o.Name == ((Item)(show_object[i])).Name; }) != null;
                collection.SetCollection(this, (Item)(show_object[i]), is_got);
            }
        }
    }

    /// <summary>
    /// 화면의 달성률 텍스트와 슬라이더 값 설정해주는 함수
    /// </summary>
    /// <param name="collection_index">설정할 Collection의 인덱스</param>
    void SetPercentInformations(int collection_index)
    {
        float got_obj_length = new Object[3][] { got_breads, got_items, got_enemys }[collection_index].Length;
        percent_text.text = $"{got_obj_length}% 수집";
        percent_slider.value = got_obj_length / UnitManager.Instance.Units.Count;
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
    /// 매개변수로 넘겨받은 index를 통해 해당 index에 맞는 종류의 Collection을 활성화
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
