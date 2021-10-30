using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DailyRewardLinker : MonoBehaviour
{
    public RectTransform Bg;
    public RectTransform Icon;
    public RectTransform Count;
    public RectTransform Already;
    public Button Get;

    DailyReward DailyReward;

    private bool is_get;
    public bool isGet
    {
        get { return is_get; }
        set
        {
            is_get = value;
            Already.gameObject.SetActive(is_get);
            //GameManager.Instance.Gets[transform.GetSiblingIndex()].gotten = is_get;
            DailyReward.gotten = is_get;
        }
    }


    private void Start()
    {
        DailyReward = GameManager.Instance.DailyRewards[transform.GetSiblingIndex()];
        var text = Count.GetComponent<TextMeshProUGUI>();
        text.text = $"{DailyReward.value}";
        //isGet = GameManager.Instance.Gets[transform.GetSiblingIndex()].gotten;
        isGet = DailyReward.gotten;
        Get.onClick.AddListener(() =>
        {
            if (transform.GetSiblingIndex() == GameManager.Instance.DailyDays && !isGet && GameManager.Instance.Daily.Date + GameManager.Instance.Daily.Time < System.DateTime.Now)
            {
                GameManager.Instance.Daily.Date = System.DateTime.Now;
                ++GameManager.Instance.DailyDays;

                isGet = true;

                switch (DailyReward.kind)
                {
                    case StageInfoLinker.Reward_Kind.Coin:
                        GameManager.Instance.Coin += DailyReward.value;
                        break;
                    case StageInfoLinker.Reward_Kind.Jem:
                        GameManager.Instance.Jem += DailyReward.value;
                        break;
                    case StageInfoLinker.Reward_Kind.Unit:
                        // NONE
                        break;
                    case StageInfoLinker.Reward_Kind.Stemina:
                        GameManager.Instance.Stemina += DailyReward.value;
                        break;
                }
            }
        });
        var img = Icon.GetComponent<Image>();

        img.sprite = DailyReward.kind switch
        {
            StageInfoLinker.Reward_Kind.Coin => UIManager.Instance.IconSprites[0],
            StageInfoLinker.Reward_Kind.Jem => UIManager.Instance.IconSprites[1],
            StageInfoLinker.Reward_Kind.Unit => null,
            StageInfoLinker.Reward_Kind.Stemina => UIManager.Instance.IconSprites[3],
            _ => null,
        };

        img.SetNativeSize();
        var glg = transform.parent.GetComponent<GridLayoutGroup>();
        var div = (glg.cellSize.y - 10) / Icon.sizeDelta.y;
        Icon.sizeDelta *= div;
    }
}
