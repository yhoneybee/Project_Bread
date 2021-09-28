using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILinker : MonoBehaviour
{
    public UnitView Viewer;

    public Image Icon;

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
                if (ButtonActions.Instance.CheckReEntering("D - 02 UnitSelect"))
                {
                    DeckManager.Select[GameManager.SelectSlotIdx] = Viewer.Show;
                    ButtonActions.Instance.ChangeScene("C - 03 DeckSelect");
                }
                else if (ButtonActions.Instance.CheckReEntering("E - 01 DeckView"))
                {
                    ButtonActions.Instance.ChangeScene("C - 03 DeckSelect");
                }
                else
                {
                    GameManager.SelectSlotIdx = UIManager.Instance.UnitViews.IndexOf(Viewer);
                    ButtonActions.Instance.ChangeScene("D - 02 UnitSelect");
                }
            });
        if (InfoBtn)
            InfoBtn.onClick.AddListener(() =>
            {
                GameManager.SelectSlotIdx = UIManager.Instance.UnitViews.IndexOf(Viewer);
                ButtonActions.Instance.ChangeScene("D - 03 UnitInfo");
            });
        foreach (var btn in Buttons)
        {
            btn.onClick.AddListener(() =>
            {
                GameManager.SelectSlotIdx = UIManager.Instance.UnitViews.IndexOf(Viewer);
                ButtonActions.Instance.ChangeScene("D - 03 UnitInfo");
            });
        }
    }
}
