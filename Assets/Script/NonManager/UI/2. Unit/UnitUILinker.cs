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
}
