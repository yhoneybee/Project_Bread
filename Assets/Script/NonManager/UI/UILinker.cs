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

    private void Start()
    {
        IconBtn.onClick.AddListener(() =>
        {
            UIManager.SelectSlot = Viewer;
            ButtonActions.Instance.ChangeScene("03 - 02 - 01 Select");
        });
        InfoBtn.onClick.AddListener(() =>
        {
            UIManager.SelectSlot = Viewer;
            ButtonActions.Instance.ChangeScene("03 - 02 - 02 Information");
        });

        foreach (var btn in Buttons)
        {
            btn.onClick.AddListener(() =>
            {
                UIManager.SelectSlot = Viewer;
                ButtonActions.Instance.ChangeScene("03 - 02 - 02 Information");
            });
        }
    }
}
