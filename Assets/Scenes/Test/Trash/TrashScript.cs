using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrashScript : MonoBehaviour
{
    [SerializeField] RectTransform rtr;
    List<Unit> Units = new List<Unit>();
    void Start()
    {
        for (int i = 0; i < 3; i++)
            Units.AddRange(Resources.LoadAll<Unit>($"Unit/Enemy/Theme {i + 1}"));

        for (int i = 0; i < rtr.childCount; i++)
        {
            rtr.GetChild(i).GetComponent<Image>().sprite = Units[i].Info.Icon;
            rtr.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = Units[i].Stat.HP.ToString();
            rtr.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = Units[i].Stat.AD.ToString();

        }
    }

    void Update()
    {

    }
}
