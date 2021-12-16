using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceLinker : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public Slider ExpSlider;
    public TextMeshProUGUI ExpText;
    public TextMeshProUGUI CoinText;
    public Button AddCoinBtn;
    public TextMeshProUGUI JemText;
    public Button AddJemBtn;
    public TextMeshProUGUI SteminaText;
    public Button AddSteminaBtn;

    private void Start()
    {
        AddCoinBtn.onClick.AddListener(() => { ButtonActions.Instance.ChangeScene("C-05_CashShop"); });
        AddJemBtn.onClick.AddListener(() => { ButtonActions.Instance.ChangeScene("C-05_CashShop"); });
        AddSteminaBtn.onClick.AddListener(() => { ButtonActions.Instance.ChangeScene("C-05_CashShop"); });
    }

    private void Update()
    {
        if (NameText && ExpSlider)
        {
            NameText.text = $"Lv.{GameManager.Instance.PlayerLevel} yhoneybee";
            ExpSlider.value = GameManager.Instance.player_exp / GameManager.Instance.need_exp;
            ExpText.text = $"{GameManager.Instance.player_exp / GameManager.Instance.need_exp * 100}%";
        }

        CoinText.text = $"{GameManager.Instance.Coin:#,0}";
        JemText.text = $"{GameManager.Instance.Jem:#,0}";
        SteminaText.text = $"{GameManager.Instance.Stemina} / {GameManager.Instance.MaxStemina}";
    }
}
