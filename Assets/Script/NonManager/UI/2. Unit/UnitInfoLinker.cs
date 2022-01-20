using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitInfoLinker : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI name_text;
    [SerializeField] TextMeshProUGUI level_text;
    [SerializeField] Image RankIcon;
    [SerializeField] Slider ExpSlider;
    [SerializeField] List<Sprite> RankSprites = new List<Sprite>();
    [SerializeField] List<RectTransform> Values = new List<RectTransform>();
    [SerializeField] ScrollRect srtUnit;
    [SerializeField] Image[] imgDots;
    [SerializeField] Button[] btnLeftRight;
    [SerializeField] Button btnUpgrade;
    [SerializeField] TextMeshProUGUI txtUpgrade;
    [SerializeField] Image imgIcon;
    [SerializeField] UnitZoom zoom;

    Coroutine c_move_panel = null;

    public int upgrade_cost;
    public int card_count;
    public int get_exp;

    int current_panel_index = 0;

    bool is_moving = false;

    void Start()
    {
        SetUpgradeValue();

        SetText();
        RankIcon.sprite = RankSprites[(int)GameManager.SelectUnit.Info.Rank];
        SetStatValue();
        //ExpSlider.maxValue = GameManager.SelectUnit.Need;

        btnLeftRight[0].onClick.AddListener(() =>
        {
            if (c_move_panel != null) StopCoroutine(c_move_panel);
            c_move_panel = StartCoroutine(MovePanel(-1));
        });

        btnLeftRight[1].onClick.AddListener(() =>
        {
            if (c_move_panel != null) StopCoroutine(c_move_panel);
            c_move_panel = StartCoroutine(MovePanel(1));
        });

        SetTextColor();
    }

    private void SetUpgradeValue()
    {
        switch (GameManager.SelectUnit.Info.Rank)
        {
            case Rank.COMMON:

                card_count = GameManager.SelectUnit.Info.Level switch
                {
                    int i when 1 <= i && i <= 10 => 20,
                    int i when 11 <= i && i <= 20 => 30,
                    int i when 21 <= i && i <= 30 => 40,
                    _ => 9999999,
                };

                upgrade_cost = GameManager.SelectUnit.Info.Level switch
                {
                    int i when 1 <= i && i <= 10 => 3000,
                    int i when 11 <= i && i <= 20 => 5000,
                    int i when 21 <= i && i <= 30 => 7000,
                    _ => 9999999,
                };

                get_exp = GameManager.SelectUnit.Info.Level switch
                {
                    int i when 1 <= i && i <= 10 => 100,
                    int i when 11 <= i && i <= 20 => 125,
                    int i when 21 <= i && i <= 30 => 150,
                    _ => 0,
                };

                break;
            case Rank.RARE:

                card_count = GameManager.SelectUnit.Info.Level switch
                {
                    int i when 1 <= i && i <= 10 => 20,
                    int i when 11 <= i && i <= 20 => 30,
                    int i when 21 <= i && i <= 30 => 40,
                    _ => 9999999,
                };

                upgrade_cost = GameManager.SelectUnit.Info.Level switch
                {
                    int i when 1 <= i && i <= 10 => 5000,
                    int i when 11 <= i && i <= 20 => 7000,
                    int i when 21 <= i && i <= 30 => 9000,
                    _ => 9999999,
                };

                get_exp = GameManager.SelectUnit.Info.Level switch
                {
                    int i when 1 <= i && i <= 10 => 200,
                    int i when 11 <= i && i <= 20 => 250,
                    int i when 21 <= i && i <= 30 => 300,
                    _ => 0,
                };

                break;
            case Rank.EPIC:

                card_count = GameManager.SelectUnit.Info.Level switch
                {
                    int i when 1 <= i && i <= 10 => 15,
                    int i when 11 <= i && i <= 20 => 25,
                    int i when 21 <= i && i <= 30 => 35,
                    _ => 9999999,
                };

                upgrade_cost = GameManager.SelectUnit.Info.Level switch
                {
                    int i when 1 <= i && i <= 10 => 7000,
                    int i when 11 <= i && i <= 20 => 9000,
                    int i when 21 <= i && i <= 30 => 11000,
                    _ => 9999999,
                };

                get_exp = GameManager.SelectUnit.Info.Level switch
                {
                    int i when 1 <= i && i <= 10 => 500,
                    int i when 11 <= i && i <= 20 => 600,
                    int i when 21 <= i && i <= 30 => 700,
                    _ => 0,
                };

                break;
            case Rank.LEGEND:

                card_count = GameManager.SelectUnit.Info.Level switch
                {
                    int i when 1 <= i && i <= 10 => 15,
                    int i when 11 <= i && i <= 20 => 25,
                    int i when 21 <= i && i <= 30 => 35,
                    _ => 9999999,
                };

                upgrade_cost = GameManager.SelectUnit.Info.Level switch
                {
                    int i when 1 <= i && i <= 10 => 9000,
                    int i when 11 <= i && i <= 20 => 11000,
                    int i when 21 <= i && i <= 30 => 13000,
                    _ => 9999999,
                };

                get_exp = GameManager.SelectUnit.Info.Level switch
                {
                    int i when 1 <= i && i <= 10 => 800,
                    int i when 11 <= i && i <= 20 => 1000,
                    int i when 21 <= i && i <= 30 => 1200,
                    _ => 0,
                };

                break;
        }
        ExpSlider.maxValue = card_count;
        int currentCount = GameManager.SelectUnit.Info.Count;
        ExpSlider.value = currentCount;
        var text = ExpSlider.GetComponentInChildren<TextMeshProUGUI>();
        text.text = $"{currentCount}/{card_count}";
        txtUpgrade.text = $"{upgrade_cost}";
    }

    void Update()
    {
        ExpSlider.value = GameManager.SelectUnit.Info.Count;

        if (Input.GetMouseButtonUp(0) && !is_moving)
        {
            if (current_panel_index == 0)
            {
                if (c_move_panel != null) StopCoroutine(c_move_panel);
                c_move_panel =
                    StartCoroutine(MovePanel(srtUnit.horizontalScrollbar.value >= 0.25f ? 1 : -1));
            }
            else
            {
                if (c_move_panel != null) StopCoroutine(c_move_panel);
                c_move_panel =
                StartCoroutine(MovePanel(srtUnit.horizontalScrollbar.value <= 0.75f ? -1 : 1));
            }
        }
    }

    private void FixedUpdate()
    {
        srtUnit.enabled = !zoom.IsHover;
        SetUpgradeValue();
    }

    void SetText()
    {
        name_text.text = GameManager.SelectUnit.Info.Name;
        level_text.text = GameManager.SelectUnit.Info.Level.ToString();
    }

    void SetStatValue()
    {
        var unit = GameManager.SelectUnit;
        ShowStatValue(0, (int)unit.Stat.AD);
        ShowStatValue(1, (int)unit.Stat.AR);
        ShowStatValue(2, (int)unit.Stat.HP);
        ShowStatValue(3, unit.Info.Cost);
        ShowStatValue(4, (int)unit.Stat.LS);
        ShowStatValue(5, (int)unit.Stat.MS);
    }

    void ShowStatValue(int index, int value)
    {
        Values[index].GetComponentInChildren<TextMeshProUGUI>().text = value.ToString();
        /*        var one_img = Values[index].GetChild(0).GetComponent<Image>();
                var ten_img = Values[index].GetChild(1).GetComponent<Image>();
                var hundred_img = Values[index].GetChild(2).GetComponent<Image>();
                var thousand_img = Values[index].GetChild(3).GetComponent<Image>();

                int one = (value % 10);
                int ten = (value % 100) / 10;
                int hundred = (value % 1000) / 100;
                int thousand = (value % 10000) / 1000;

                int one_idx = one == 0 ? 9 : one - 1;
                int ten_idx = ten == 0 ? 9 : ten - 1;
                int hundred_idx = hundred == 0 ? 9 : hundred - 1;
                int thousand_idx = thousand == 0 ? 9 : thousand - 1;

                one_img.gameObject.SetActive(true);
                ten_img.gameObject.SetActive(true);
                hundred_img.gameObject.SetActive(true);
                thousand_img.gameObject.SetActive(true);

                one_img.sprite = UIManager.Instance.Nums[one_idx];
                ten_img.sprite = UIManager.Instance.Nums[ten_idx];
                hundred_img.sprite = UIManager.Instance.Nums[hundred_idx];
                thousand_img.sprite = UIManager.Instance.Nums[thousand_idx];

                if (thousand_idx == 9) thousand_img.gameObject.SetActive(false);
                if (hundred_idx == 9 && thousand_idx == 9) hundred_img.gameObject.SetActive(false);
                if (ten_idx == 9 && hundred_idx == 9 && thousand_idx == 9) ten_img.gameObject.SetActive(false);*/
    }

    public void Upgrade_Unit()
    {
        if (GameManager.Instance.Coin >= upgrade_cost && GameManager.SelectUnit.Info.Count >= card_count && GameManager.SelectUnit.Info.Level < 30)
        {
            GameManager.Instance.Coin -= upgrade_cost;
            GameManager.SelectUnit.Info.Level++;
            GameManager.SelectUnit.Info.Count -= card_count;
            GameManager.Instance.PlayerExp += get_exp;

            SetText();

            SetTextColor();
        }
    }

    private void SetTextColor()
    {
        imgIcon.color = txtUpgrade.color = btnUpgrade.targetGraphic.color = new Color(0.509434f, 0.509434f, 0.509434f, 1);

        if (GameManager.SelectUnit.Info.Count >= card_count && GameManager.Instance.Coin < upgrade_cost)
        {
            imgIcon.color = btnUpgrade.targetGraphic.color = Color.white;
            txtUpgrade.color = Color.red;
        }
        else if (GameManager.Instance.Coin >= upgrade_cost && GameManager.SelectUnit.Info.Count >= card_count)
        {
            imgIcon.color = txtUpgrade.color = btnUpgrade.targetGraphic.color = Color.white;
        }
    }

    /// <summary>
    /// 유닛 정보 창 좌우로 이동 시키는 함수
    /// </summary>
    /// <param name="direction_x"></param>
    /// <returns></returns>
    IEnumerator MovePanel(int direction_x)
    {
        is_moving = true;
        int target_value = direction_x == 1 ? 1 : 0;

        while (true)
        {
            if (Mathf.Abs(srtUnit.horizontalScrollbar.value - target_value) <= 0.01f)
            {
                srtUnit.horizontalScrollbar.value = target_value;
                break;
            }
            srtUnit.horizontalScrollbar.value = Mathf.MoveTowards(srtUnit.horizontalScrollbar.value, target_value, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        imgDots[1 - target_value].color = Color.white;
        imgDots[target_value].color = new Color(0.5f, 0.5f, 0.5f);

        btnLeftRight[1 - target_value].gameObject.SetActive(true);
        btnLeftRight[target_value].gameObject.SetActive(false);

        current_panel_index = target_value;
        is_moving = false;
    }
}
