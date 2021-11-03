using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageTitleLinker : MonoBehaviour
{
    public Image ThemeText;
    public Sprite[] ThemeTexts = new Sprite[3];

    public Image ThemeNum;
    public Image NumTen;
    public Image NumOne;

    public Image[] Stars = new Image[3];
    public SwitchSprite StarSprite;

    public Sprite[] Nums = new Sprite[10];

    private void Start()
    {
        var data = StageManager.Instance.GetStage();
        ThemeText.sprite = ThemeTexts[StageInfo.theme_number - 1];

        ThemeNum.sprite = Nums[StageInfo.theme_number - 1];
        int ten = StageInfo.stage_number / 10;
        int one = StageInfo.stage_number % 10;

        int ten_idx = ten == 0 ? 9 : ten - 1;
        int one_idx = one == 0 ? 9 : one - 1;

        NumOne.gameObject.SetActive(true);

        if (ten_idx == 9)
        {
            NumTen.sprite = Nums[one_idx];
            NumOne.gameObject.SetActive(false);
        }
        else
        {
            NumTen.sprite = Nums[ten_idx];
            NumOne.sprite = Nums[one_idx];
        }

        for (int i = 0; i < 3; i++)
        {
            if (i < data.star_count) Stars[i].sprite = StarSprite.ASprite;
            else Stars[i].sprite = StarSprite.BSprite;
        }
    }
}
