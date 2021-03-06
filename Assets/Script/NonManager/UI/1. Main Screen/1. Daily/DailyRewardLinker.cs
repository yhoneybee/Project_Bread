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
    public bool isResult;

    DailyReward DailyReward;

    private bool is_get;
    public bool isGet
    {
        get { return is_get; }
        set
        {
            is_get = value;
            Already.gameObject.SetActive(is_get);
            DailyReward.gotten = is_get;
        }
    }

    private void Start()
    {
        Get.onClick.AddListener(() =>
        {
            UIManager.Instance.DailyUI.GetDailyParent.gameObject.SetActive(true);
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
        });
    }

    private void FixedUpdate()
    {
        int thisIndex = transform.GetSiblingIndex();
        DailyReward = GameManager.Instance.DailyRewards[isResult ? (GameManager.Instance.DailyDays == 0 ? 0 : GameManager.Instance.DailyDays - 1) : thisIndex];
        isGet = DailyReward.gotten;

        var img = Icon.GetComponent<Image>();

        img.sprite = DailyReward.kind switch
        {
            StageInfoLinker.Reward_Kind.Coin => UIManager.Instance.IconSprites[0],
            StageInfoLinker.Reward_Kind.Jem => UIManager.Instance.IconSprites[1],
            StageInfoLinker.Reward_Kind.Unit => null,
            StageInfoLinker.Reward_Kind.Stemina => UIManager.Instance.IconSprites[3],
            _ => null,
        };

        var text = Count.GetComponent<TextMeshProUGUI>();
        text.text = $"{DailyReward.value}";

        var glg = transform.parent.GetComponent<GridLayoutGroup>();

        UIManager.Instance.FixSizeToRatio(img, isResult ? 100 : glg.cellSize.y - 10);

        string shortDate = GameManager.Instance.Daily.Date.ToString("dd");
        string nowShortDate = System.DateTime.Now.ToString("dd");
        int iShortDate = System.Convert.ToInt32(shortDate);
        int iNowShortDate = System.Convert.ToInt32(nowShortDate);
        if (isResult)
        {
            Already.gameObject.SetActive(false);
            UIManager.Instance.DailyUI.GetDailyParent.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{DailyReward.kind}??(??) ??????????????!";
            if (Input.GetMouseButton(0))
            {
                UIManager.Instance.DailyUI.GetDailyParent.gameObject.SetActive(false);
                if (GameManager.Instance.DailyDays == 28)
                {
                    foreach (var reward in GameManager.Instance.DailyRewards)
                        reward.gotten = false;

                    GameManager.Instance.DailyDays = 0;
                }
                SaveManager.SaveValue(GameManager.Instance.DailyDays, "DailyDays");
                SaveManager.SaveEnumerable(GameManager.Instance.DailyRewards, "DailyRewards");
            }
        }
        else
        {
            if (thisIndex != GameManager.Instance.DailyDays || isGet || (iShortDate >= iNowShortDate && (iShortDate != 30 || iNowShortDate >= 30) && (iShortDate != 31 || iNowShortDate >= 31)))
            {
                GetComponent<Animator>().Play("NONE");
                Get.enabled = false;
                return;
            }

            Get.enabled = true;
            GetComponent<Animator>().Play("Pointing");
        }
    }

    /*    private void FixedUpdate()
        {
            if (!isResult)
            {
                int this_index = transform.GetSiblingIndex();

                DailyReward = GameManager.Instance.DailyRewards[this_index];
                var text = Count.GetComponent<TextMeshProUGUI>();
                text.text = $"{DailyReward.value}";
                //isGet = GameManager.Instance.Gets[transform.GetSiblingIndex()].gotten;
                isGet = DailyReward.gotten;
                string shortDate = GameManager.Instance.Daily.Date.ToString("dd");
                string nowShortDate = System.DateTime.Now.ToString("dd");
                int iShortDate = System.Convert.ToInt32(shortDate);
                int iNowShortDate = System.Convert.ToInt32(nowShortDate);
                if (this_index == GameManager.Instance.DailyDays && !isGet && (iShortDate < iNowShortDate || (iShortDate == 30 && iNowShortDate < 30) || (iShortDate == 31 && iNowShortDate < 31)))
                    GetComponent<Animator>().Play("Pointing");
                Get.onClick.AddListener(() =>
                {
                    string shortDate = GameManager.Instance.Daily.Date.ToString("dd");
                    string nowShortDate = System.DateTime.Now.ToString("dd");
                    int iShortDate = System.Convert.ToInt32(shortDate);
                    int iNowShortDate = System.Convert.ToInt32(nowShortDate);
                    if (this_index == GameManager.Instance.DailyDays && !isGet && (iShortDate < iNowShortDate || (iShortDate == 30 && iNowShortDate < 30) || (iShortDate == 31 && iNowShortDate < 31)))
                    {
                        GetComponent<Animator>().Play("NONE");
                        UIManager.Instance.DailyUI.GetDailyParent.gameObject.SetActive(true);
                        GameManager.Instance.Daily.Date = System.DateTime.Now;
                        ++GameManager.Instance.DailyDays;
                        SaveManager.SaveValue(GameManager.Instance.DailyDays, "DailyDays");

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
            }
            else
            {
                DailyReward = GameManager.Instance.DailyRewards[GameManager.Instance.DailyDays];
                var text = Count.GetComponent<TextMeshProUGUI>();
                text.text = $"{DailyReward.value}";
                UIManager.Instance.DailyUI.GetDailyParent.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{DailyReward.kind}??(??) ??????????????!";
                {
                }
            }

            var img = Icon.GetComponent<Image>();

            img.sprite = DailyReward.kind switch
            {
                StageInfoLinker.Reward_Kind.Coin => UIManager.Instance.IconSprites[0],
                StageInfoLinker.Reward_Kind.Jem => UIManager.Instance.IconSprites[1],
                StageInfoLinker.Reward_Kind.Unit => null,
                StageInfoLinker.Reward_Kind.Stemina => UIManager.Instance.IconSprites[3],
                _ => null,
            };

            var glg = transform.parent.GetComponent<GridLayoutGroup>();

            UIManager.Instance.FixSizeToRatio(img, isResult ? 100 : glg.cellSize.y - 10);
        }*/
}
