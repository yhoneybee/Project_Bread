using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitInfoLinker : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI name_text;
    [SerializeField] TextMeshProUGUI level_text;

    public int upgrade_cost;
    public int card_count;

    void Start()
    {
        SetText();
    }

    void Update()
    {

    }
    void SetText()
    {
        name_text.text = GameManager.SelectUnit.Info.Name;
        level_text.text = GameManager.SelectUnit.Info.Level.ToString();
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
