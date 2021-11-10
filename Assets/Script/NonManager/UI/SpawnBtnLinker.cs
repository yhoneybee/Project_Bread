using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum eSPAWN_COUNT
{
    TEN,
    ONE,
}

public class SpawnBtnLinker : MonoBehaviour
{
    public static int Jem;
    public static int Coin;

    public StageInfoLinker.Reward_Kind CostType;
    public int value;

    public TextMeshProUGUI txtReward;
    public Image imgIcon;

    public eSPAWN_COUNT eSpawnCount;
    public int rank_value;
    Button button;

    private void Start()
    {
        imgIcon.sprite = UIManager.Instance.spIcon[((int)CostType)];
        txtReward.text = $"{value}";

        button = GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            bool isUnboxing = false;
            switch (CostType)
            {
                case StageInfoLinker.Reward_Kind.Coin:
                    if (GameManager.Instance.Coin >= value)
                    {
                        isUnboxing = true;
                        Coin = value;
                    }
                    break;
                case StageInfoLinker.Reward_Kind.Jem:
                    if (GameManager.Instance.Jem >= value)
                    {
                        isUnboxing = true;
                        Jem = value;
                    }
                    break;
            }

            if (isUnboxing)
            {
                switch (eSpawnCount)
                {
                    case eSPAWN_COUNT.TEN:
                        ButtonActions.Instance.UnBoxingTen(rank_value);
                        break;
                    case eSPAWN_COUNT.ONE:
                        ButtonActions.Instance.UnBoxingOne(rank_value);
                        break;
                }
            }
            else
            {
                // TODO : µ·¾øÀ½ ¶ç¿ì±â
            }
        });
    }
}
