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
    [SerializeField] Zoom zoom;

    public int upgrade_cost;
    public int card_count;

    private float srtUnitPrevValue;
    private float srtUnitValue;

    void Start()
    {
        SetText();
        RankIcon.sprite = RankSprites[(int)GameManager.SelectUnit.Info.Rank];
        SetStatValue();
        ExpSlider.maxValue = GameManager.SelectUnit.Need;

        btnLeftRight[0].onClick.AddListener(() =>
        {
            srtUnit.horizontalScrollbar.value = 0;
        });

        btnLeftRight[1].onClick.AddListener(() =>
        {
            srtUnit.horizontalScrollbar.value = 1;
        });

        SetTextColor();
    }

    void Update()
    {
        ExpSlider.value = GameManager.SelectUnit.Info.Count;
    }

    private void FixedUpdate()
    {
        srtUnitPrevValue = srtUnitValue;
        srtUnitValue = srtUnit.horizontalScrollbar.value;

        srtUnit.enabled = !zoom.IsHover;

        if (!zoom.IsHover)
        {
            if (Mathf.Abs(srtUnitValue - srtUnitPrevValue) <= 0.05f)
            {
                if (srtUnitValue > 0.5f)
                {
                    srtUnit.horizontalScrollbar.value = Mathf.Lerp(srtUnit.horizontalScrollbar.value, 1, Time.deltaTime * 3);
                    imgDots[1].color = Color.white;
                    imgDots[0].color = new Color(0.509804f, 0.509804f, 0.509804f, 1);
                    btnLeftRight[0].gameObject.SetActive(true);
                    btnLeftRight[1].gameObject.SetActive(false);
                }
                else
                {
                    srtUnit.horizontalScrollbar.value = Mathf.Lerp(srtUnit.horizontalScrollbar.value, 0, Time.deltaTime * 3);
                    imgDots[0].color = Color.white;
                    imgDots[1].color = new Color(0.509804f, 0.509804f, 0.509804f, 1);
                    btnLeftRight[1].gameObject.SetActive(true);
                    btnLeftRight[0].gameObject.SetActive(false);
                }
            }
        }
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
        var one_img = Values[index].GetChild(0).GetComponent<Image>();
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
        if (hundred_idx == 9) hundred_img.gameObject.SetActive(false);
        if (ten_idx == 9) ten_img.gameObject.SetActive(false);
    }

    public void Upgrade_Unit()
    {
        if (GameManager.Instance.Coin >= upgrade_cost && GameManager.SelectUnit.Info.Count >= card_count)
        {
            GameManager.Instance.Coin -= upgrade_cost;
            GameManager.SelectUnit.Info.Level++;
            GameManager.SelectUnit.Info.Count -= card_count;

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
}
