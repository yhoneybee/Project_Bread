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

    public int upgrade_cost;
    public int card_count;

    void Start()
    {
        SetText();
        RankIcon.sprite = RankSprites[(int)GameManager.SelectUnit.Info.Rank];
        SetStatValue();
        ExpSlider.maxValue = GameManager.SelectUnit.Need;
    }

    void Update()
    {
        ExpSlider.value = GameManager.SelectUnit.Info.Count;
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

        int one = (value % 10);
        int ten = (value % 100) / 10;
        int hundred = value / 100;

        int one_idx = one == 0 ? 9 : one - 1;
        int ten_idx = ten == 0 ? 9 : ten - 1;
        int hundred_idx = hundred == 0 ? 9 : hundred - 1;

        one_img.gameObject.SetActive(true);
        ten_img.gameObject.SetActive(true);
        hundred_img.gameObject.SetActive(true);

        one_img.sprite = UIManager.Instance.Nums[one_idx];
        ten_img.sprite = UIManager.Instance.Nums[ten_idx];
        hundred_img.sprite = UIManager.Instance.Nums[hundred_idx];

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
        }
    }
}
