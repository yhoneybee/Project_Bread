using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitUILinker : MonoBehaviour
{
    public UnitView Viewer;

    public Image Icon;
    public Image imgRank;
    public Image imgRankBg;
    public RectTransform IconRestore;

    public Slider CardSlider;
    public TextMeshProUGUI textCard;

    public Button IconBtn;
    public Button InfoBtn;
    public Button[] Buttons;

    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI NameText;

    public GameObject NullUnActive;

    private void Start()
    {
        if (IconBtn)
            IconBtn.onClick.AddListener(() =>
            {
                print(ButtonActions.directMain);
                if (ButtonActions.directMain) return;
                if (ButtonActions.Instance.CheckReEntering("D-02_UnitSelect"))
                {
                    DeckManager.Select[GameManager.SelectSlotIdx] = Viewer.Show;
                    ButtonActions.Instance.ChangeScene("C-03_DeckSelect");
                }
                else if (ButtonActions.Instance.CheckReEntering("E-01_DeckView"))
                {
                    ButtonActions.Instance.ChangeScene("C-03_DeckSelect");
                }
                else
                {
                    GameManager.SelectSlotIdx = UIManager.Instance.UnitViews.IndexOf(Viewer);
                    GameManager.SelectUnit = Viewer.Show;
                    ButtonActions.Instance.ChangeScene("D-02_UnitSelect");
                }
            });
        if (InfoBtn)
            InfoBtn.onClick.AddListener(() =>
            {
                GameManager.SelectSlotIdx = UIManager.Instance.UnitViews.IndexOf(Viewer);
                GameManager.SelectUnit = Viewer.Show;
                ButtonActions.Instance.ChangeScene("D-04_UnitInfo");
            });
        foreach (var btn in Buttons)
        {
            btn.onClick.AddListener(() =>
            {
                GameManager.SelectSlotIdx = UIManager.Instance.UnitViews.IndexOf(Viewer);
                GameManager.SelectUnit = Viewer.Show;
                ButtonActions.Instance.ChangeScene("D-03_UnitItemInfo");
            });
        }
    }

    private void FixedUpdate()
    {
        if (CardSlider) CardSlider.gameObject.SetActive(Viewer.Show);
        if (!CardSlider || !Viewer.Show) return;
        int card_count = 0;

        switch (Viewer.Show.Info.Rank)
        {
            case Rank.COMMON:

                card_count = Viewer.Show.Info.Level switch
                {
                    int i when 1 <= i && i <= 10 => 20,
                    int i when 11 <= i && i <= 20 => 30,
                    int i when 21 <= i && i <= 30 => 40,
                    _ => 9999999,
                };

                break;
            case Rank.RARE:

                card_count = Viewer.Show.Info.Level switch
                {
                    int i when 1 <= i && i <= 10 => 20,
                    int i when 11 <= i && i <= 20 => 30,
                    int i when 21 <= i && i <= 30 => 40,
                    _ => 9999999,
                };

                break;
            case Rank.EPIC:

                card_count = Viewer.Show.Info.Level switch
                {
                    int i when 1 <= i && i <= 10 => 15,
                    int i when 11 <= i && i <= 20 => 25,
                    int i when 21 <= i && i <= 30 => 35,
                    _ => 9999999,
                };

                break;
            case Rank.LEGEND:

                card_count = Viewer.Show.Info.Level switch
                {
                    int i when 1 <= i && i <= 10 => 15,
                    int i when 11 <= i && i <= 20 => 25,
                    int i when 21 <= i && i <= 30 => 35,
                    _ => 9999999,
                };

                break;
        }

        CardSlider.maxValue = card_count;
        int currentCount = Viewer.Show.Info.Count;
        CardSlider.value = currentCount;
        textCard.text = $"{currentCount}/{card_count}";
    }
}
