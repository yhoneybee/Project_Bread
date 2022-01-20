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
            ExpSlider.value = GameManager.Instance.PlayerExp;
            ExpSlider.maxValue = GameManager.Instance.selectLevelUpEffect.needExp;
            ExpText.text = $"{GameManager.Instance.PlayerExp * 100 / GameManager.Instance.levelUpEffects.Find(x => x.minLevel <= GameManager.Instance.PlayerLevel && GameManager.Instance.PlayerLevel <= x.maxLevel).needExp}%";

            // exp : need = x : 100

            if (GameManager.Instance.PlayerLevel >= 100)
            {
                ExpSlider.value = 1;
                ExpSlider.maxValue = 1;
                ExpText.text = $"MAX";
            }
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            GameManager.Instance.PlayerExp += 100;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            GameManager.Instance.PlayerExp += 1000;
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            GameManager.Instance.PlayerExp += 10000;
        }

        CoinText.text = $"{GameManager.Instance.Coin:#,0}";
        JemText.text = $"{GameManager.Instance.Jem:#,0}";
        SteminaText.text = $"{GameManager.Instance.Stemina} / {GameManager.Instance.MaxStemina}";
    }
}
