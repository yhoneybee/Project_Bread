using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

// 대호 개발, 업적 씬을 총괄하는 클래스
public class AchievementLinker : MonoBehaviour, IComparer<Achieve>
{
    [SerializeField] GameObject[] achieves_scrollView = new GameObject[4];
    [Space(20)]

    [SerializeField] RectTransform[] adventure_parent = new RectTransform[2];
    [SerializeField] RectTransform[] content_parent = new RectTransform[2];
    [SerializeField] RectTransform[] system_parent = new RectTransform[2];
    [SerializeField] RectTransform[] character_parent = new RectTransform[2];
    [Space(20)]

    [SerializeField] List<Achieve> all_achieves;
    [Space(20)]

    [SerializeField] Button[] select_achieveList_button = new Button[4];
    [Space(20)]

    [SerializeField] Button[] achive_case_button = new Button[2];
    [SerializeField] Sprite[] none_button_sprite;
    [SerializeField] Sprite[] taked_button_sprite;

    enum ActiveAchieveCase
    {
        None = 0,
        Taked = 1
    }
    ActiveAchieveCase active_achieve_case = ActiveAchieveCase.None;

    int show_button_index = 0;
    int active_achieve_index = 0;

    private void Start()
    {
        foreach (var achieve in all_achieves)
        {
            achieve.Setting();
        }

        for (int i = 0; i < select_achieveList_button.Length; i++)
        {
            if (i != show_button_index)
                select_achieveList_button[i].image.color = new Color(0.5f, 0.5f, 0.5f);
        }

        SetAchieveLists();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            foreach (var achieve in all_achieves)
            {
                PlayerPrefs.SetInt(achieve.id.ToString(), 0);
            }
        }
    }

    void SetAchieveLists()
    {
        all_achieves.Sort(Compare);

        List<Achieve>[] target_achieves = new List<Achieve>[4] { new List<Achieve>(), new List<Achieve>(), new List<Achieve>(), new List<Achieve>() };

        RectTransform[] parents = new RectTransform[4] { adventure_parent[1], content_parent[1], system_parent[1], character_parent[1] };

        var target_achieve_list = from achieve in all_achieves
                                  group achieve by achieve.achieve_type into g
                                  select new { type = g.Key, achieves = g };

        foreach (var target_achieve in target_achieve_list)
        {
            foreach (var achieve in target_achieve.achieves)
            {
                target_achieves[(int)target_achieve.type].Add(achieve);
            }
        }

        for (int i = 0; i < 4; i++)
        {
            var taked_achieves = from achieves in target_achieves[i]
                                 where achieves.achieve_condition == Achieve.AchieveCondition.Taked
                                 select achieves;

            foreach (var taked_achieve in taked_achieves)
            {
                taked_achieve.transform.parent = parents[i];
            }
        }
    }

    /// <summary>
    /// 4개의 업적 목록 중 index에 맞는 목록을 보여줌
    /// </summary>
    /// <param name="index"></param>
    public void ShowAhcieveList(int index)
    {
        // 기존 창 비활성화
        achieves_scrollView[show_button_index].SetActive(false);

        // 선택 창 활성화
        achieves_scrollView[index].SetActive(true);

        select_achieveList_button[show_button_index].image.color = new Color(0.5f, 0.5f, 0.5f);

        select_achieveList_button[index].image.color = Color.white;

        show_button_index = index;

        ShowAchieveCase(active_achieve_index);
    }
    /// <summary>
    /// 업적 / 완료 업적 구분해서 활성화해주는 함수
    /// </summary>
    /// <param name="case_number">업적 : 0, 완료 업적 : 1</param>
    public void ShowAchieveCase(int case_number)
    {
        int current_case_number = Mathf.Abs(case_number - 1);

        RectTransform[] current_parents = new RectTransform[4][]
        { adventure_parent, content_parent,
            system_parent, character_parent }[show_button_index];

        current_parents[current_case_number].gameObject.SetActive(false);

        Sprite[] target_buttons = new Sprite[4]
        { none_button_sprite[0], none_button_sprite[1],
            taked_button_sprite[0], taked_button_sprite[1] };

        // 기존 버튼 Sprite 설정
        achive_case_button[active_achieve_index].image.sprite =
            target_buttons[active_achieve_index * 2];

        active_achieve_case = (ActiveAchieveCase)case_number;
        active_achieve_index = (int)active_achieve_case;

        // 선택한 버튼 Sprite 설정
        achive_case_button[active_achieve_index].image.sprite =
            target_buttons[1 + active_achieve_index * 2];

        current_parents[active_achieve_index].gameObject.SetActive(true);
    }

    public int Compare(Achieve x, Achieve y)
    {
        if (x.achieve_condition == Achieve.AchieveCondition.Takable)
        {
            return -1;
        }
        else if (x.achieve_condition == Achieve.AchieveCondition.None)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
}