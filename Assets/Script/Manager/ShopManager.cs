using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; } = null;

    [SerializeField] Image[] ButtonImgs = new Image[6];
    [SerializeField] Image[] BubbleMessage = new Image[3];
    TextMeshProUGUI[] Timer = new TextMeshProUGUI[3];

    readonly string FREE_SAPWN = "무료뽑기 까지\n";

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        for (int i = 0; i < 3; i++) Timer[i] = BubbleMessage[i].GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            Timer[i].text = $"{FREE_SAPWN}";
        }
    }
}
