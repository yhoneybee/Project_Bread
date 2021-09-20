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
        IconBtn.onClick.AddListener(() =>
        {
            UIManager.SelectSlot = Viewer;
            ButtonActions.Instance.ChangeScene("D - 02 UnitSelect");
        });
        InfoBtn.onClick.AddListener(() =>
        {
            UIManager.SelectSlot = Viewer;
            ButtonActions.Instance.ChangeScene("D - 03 UnitInfo");
        });
        foreach (var btn in Buttons)
        {
            btn.onClick.AddListener(() =>
            {
                UIManager.SelectSlot = Viewer;
                ButtonActions.Instance.ChangeScene("D - 03 UnitInfo");
            });
        }
    }
}
