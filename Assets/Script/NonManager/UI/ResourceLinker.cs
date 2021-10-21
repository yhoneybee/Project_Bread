using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceLinker : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI CoinText;
    public Button AddCoinBtn;
    public TextMeshProUGUI JemText;
    public Button AddJemBtn;
    public TextMeshProUGUI SteminaText;
    public Button AddSteminaBtn;

    private void Update()
    {
        if (NameText)
            NameText.text = $"Lv.{GameManager.Instance.player_level} yhoneybee";

        CoinText.text = $"{GameManager.Instance.Coin}";
        JemText.text = $"{GameManager.Instance.Jem}";
        SteminaText.text = $"{GameManager.Instance.Stemina} / {GameManager.Instance.MaxStemina}";
    }
}
