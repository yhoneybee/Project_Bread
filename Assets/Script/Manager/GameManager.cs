using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using TMPro;
using System.Threading.Tasks;

[Serializable]
public class DailyReward
{
    public StageInfoLinker.Reward_Kind kind;
    public int value;
    public bool gotten;
}

[Serializable]
public class LevelUpEffect
{
    public int minLevel;
    public int maxLevel;
    public int needExp;
    public int rewardC;
    public int rewardJ;
}

[Serializable]
public class RewardCount
{
    public int JemRewardCount;
    public int CoinRewardCount;
    public int SteminaRewardCount;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } = null;

    public delegate void OnAutoSave();
    OnAutoSave on_auto_save = () => { print("AUTO SAVING..."); };

    public event OnAutoSave onAutoSave
    {
        add { on_auto_save += value; value(); }
        remove { on_auto_save -= value; }
    }


    public List<DailyReward> DailyRewards = new List<DailyReward>();

    public List<List<Unit>> Decks { get; private set; } = new List<List<Unit>>()
    {
        new List<Unit>(){ null,null,null,null,null,null,null, },
        new List<Unit>(){ null,null,null,null,null,null,null, },
        new List<Unit>(){ null,null,null,null,null,null,null, },
        new List<Unit>(){ null,null,null,null,null,null,null, },
        new List<Unit>(){ null,null,null,null,null,null,null, },
        new List<Unit>(){ null,null,null,null,null,null,null, },
        new List<Unit>(){ null,null,null,null,null,null,null, },
        new List<Unit>(){ null,null,null,null,null,null,null, },
        new List<Unit>(){ null,null,null,null,null,null,null, },
    };

    public static List<Unit> Select => Instance.Decks[Instance.Index];

    public List<Item> Items = new List<Item>();

    private int index = 0;
    public int Index
    {
        get { return index; }
        set
        {
            index = value;
            if (DeckManager.Instance) DeckManager.Instance.DeckApply();
        }
    }

    public static Unit SelectUnit;
    public static int SelectSlotIdx;

    public int GlobalCP;
    // LS Persent
    public int GlobalLSP;
    // 피흡 비율
    public int LifeStealRatio;
    // 부활 확률
    public int ResurrectionP;

    private int playerLevel = 1;
    public int PlayerLevel
    {
        get => playerLevel;
        set
        {
            playerLevel = value;
            // TODO : 1레벨당 추가 효과
            selectLevelUpEffect = levelUpEffects.Find(x => x.minLevel <= playerLevel && playerLevel <= x.maxLevel);
            var levelup = GameObject.Find("LevelUp").GetComponent<LevelUpLinker>();
            levelup.txtCoinCount.text = $"{selectLevelUpEffect.rewardC:#,0}";
            levelup.txtJemCount.text = $"{selectLevelUpEffect.rewardJ:#,0}";
            levelup.rtrnRealParent.gameObject.SetActive(true);
            if (selectLevelUpEffect != null)
            {
                Coin += selectLevelUpEffect.rewardC;
                Jem += selectLevelUpEffect.rewardJ;
            }
            if (playerLevel % 10 == 0)
            {
                // TODO : 10레벨 추가 효과, 아마 문서로 정리되야 할듯
                switch (playerLevel / 10)
                {
                    case 1:
                        GlobalCP = 10;
                        break;
                    case 2:
                        GlobalLSP = 5;
                        LifeStealRatio = 5;
                        break;
                    case 3:
                        ResurrectionP = 2;
                        break;
                    case 4:
                        GlobalCP = 20;
                        break;
                    case 5:
                        GlobalLSP = 10;
                        LifeStealRatio = 10;
                        break;
                    case 6:
                        ResurrectionP = 4;
                        break;
                    case 7:
                        GlobalCP = 30;
                        break;
                    case 8:
                        GlobalLSP = 15;
                        LifeStealRatio = 15;
                        break;
                    case 9:
                        ResurrectionP = 6;
                        break;
                    case 10:
                        GlobalCP = 45;
                        GlobalLSP = 20;
                        LifeStealRatio = 20;
                        ResurrectionP = 10;
                        break;
                }
            }
        }
    }
    public int PlayerExp
    {
        get => playerExp;
        set
        {
            if (PlayerLevel >= 100) return;

            selectLevelUpEffect = levelUpEffects.Find(x => x.minLevel <= PlayerLevel && PlayerLevel <= x.maxLevel);
            playerExp = value;
            if (selectLevelUpEffect.needExp <= playerExp)
            {
                PlayerExp -= selectLevelUpEffect.needExp;
                PlayerLevel++;
            }
        }
    }
    private int playerExp;
    public int Coin = 0;
    public int Jem = 0;
    public int Stemina = 20;
    public int MaxStemina = 20;
    public int UnBoxingCount = 11;
    public int GameCount
    {
        get => gameCount;
        set
        {
            if (value % 2 == 0) AdmobManager.Instance.ShowInterstitial();
            gameCount = value;
        }
    }
    private int gameCount;

    public bool EnteredDeckView = false;

    public readonly int theme_count = 3;

    public List<DateTimer> DateTimers = new List<DateTimer>(3);

    public DateTimer Daily = new DateTimer { Time = new TimeSpan(24, 0, 0) };

    public DateTime lastLogin;

    private int daily_days = 0;
    public int DailyDays
    {
        get { return daily_days; }
        set
        {
            daily_days = value;
            //daily_days %= 28;
        }
    }

    public RewardCount RewardCount;

    public List<LevelUpEffect> levelUpEffects = new List<LevelUpEffect>();

    public LevelUpEffect selectLevelUpEffect;

    [SerializeField] ParticleSystem touch_up_effect_prefab;
    ParticleSystem touch_up_effect;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        levelUpEffects.Add(new LevelUpEffect { minLevel = 1, maxLevel = 10, needExp = 500, rewardC = 1000, rewardJ = 10 });
        levelUpEffects.Add(new LevelUpEffect { minLevel = 11, maxLevel = 20, needExp = 600, rewardC = 2000, rewardJ = 20 });
        levelUpEffects.Add(new LevelUpEffect { minLevel = 21, maxLevel = 30, needExp = 800, rewardC = 3000, rewardJ = 30 });
        levelUpEffects.Add(new LevelUpEffect { minLevel = 31, maxLevel = 40, needExp = 1100, rewardC = 4000, rewardJ = 40 });
        levelUpEffects.Add(new LevelUpEffect { minLevel = 41, maxLevel = 50, needExp = 1500, rewardC = 5000, rewardJ = 50 });
        levelUpEffects.Add(new LevelUpEffect { minLevel = 51, maxLevel = 60, needExp = 2000, rewardC = 6000, rewardJ = 60 });
        levelUpEffects.Add(new LevelUpEffect { minLevel = 61, maxLevel = 70, needExp = 2600, rewardC = 7000, rewardJ = 70 });
        levelUpEffects.Add(new LevelUpEffect { minLevel = 71, maxLevel = 80, needExp = 3300, rewardC = 8000, rewardJ = 80 });
        levelUpEffects.Add(new LevelUpEffect { minLevel = 81, maxLevel = 90, needExp = 4100, rewardC = 9000, rewardJ = 90 });
        levelUpEffects.Add(new LevelUpEffect { minLevel = 91, maxLevel = 100, needExp = 5000, rewardC = 10000, rewardJ = 100 });

        selectLevelUpEffect = levelUpEffects[0];

        StartCoroutine(EAutoSave());

        var resource = SaveManager.Load<int>("ResourceData");

        RewardCount.JemRewardCount = 3;
        RewardCount.CoinRewardCount = 3;
        RewardCount.SteminaRewardCount = 3;

        if (SaveManager.Instance.IsFile("RewardCount"))
        {
            RewardCount = SaveManager.LoadValue<RewardCount>("RewardCount");
        }

        if (SaveManager.Instance.IsFile("LastLogin"))
        {
            lastLogin = DateTime.Parse(SaveManager.LoadValue<string>("LastLogin"));
            if (lastLogin < DateTime.Parse(DateTime.Now.ToString("d")))
            {
                lastLogin = DateTime.Now;
                RewardCount.JemRewardCount = 3;
                RewardCount.CoinRewardCount = 3;
                RewardCount.SteminaRewardCount = 3;
            }
        }
        else
        {
            lastLogin = DateTime.Now;
            RewardCount.JemRewardCount = 3;
            RewardCount.CoinRewardCount = 3;
            RewardCount.SteminaRewardCount = 3;
        }

        onAutoSave += () => { SaveManager.SaveValue(RewardCount, "RewardCount"); };

        SaveManager.SaveValue(lastLogin.ToString("d"), "LastLogin");

        if (resource != null && resource.Count() > 0)
        {
            Coin = resource.ElementAt(0);
            Jem = resource.ElementAt(1);
            Stemina = resource.ElementAt(2);
            MaxStemina = resource.ElementAt(3);
        }

        onAutoSave += () => { SaveManager.SaveEnumerable(new List<int>() { Coin, Jem, Stemina, MaxStemina }, "ResourceData"); };

        DailyDays = SaveManager.LoadValue<int>("DailyDays");

        onAutoSave += () => { SaveManager.SaveValue(DailyDays, "DailyDays"); };

        if (SaveManager.Instance.IsFile($"DateTimers_Date"))
        {
            var dates = SaveManager.Load<string>("DateTimers_Date");
            var times = SaveManager.Load<string>("DateTimers_Time");
            for (int i = 0; i < 3; i++)
            {
                DateTimers.Add(new DateTimer());
                DateTimers[i].Date = DateTime.Parse(dates.ElementAt(i));
                DateTimers[i].Time = TimeSpan.Parse(times.ElementAt(i));
            }
        }
        else
        {
            for (int i = 0; i < 3; i++) DateTimers.Add(new DateTimer { Date = DateTime.Now, Time = new TimeSpan(0, 30 * (i + 1), 0) });
        }

        onAutoSave += () => { SaveManager.SaveEnumerable(DateTimers.Select((o) => o.Date.ToString()), "DateTimers_Date"); };
        onAutoSave += () => { SaveManager.SaveEnumerable(DateTimers.Select((o) => o.Time.ToString()), "DateTimers_Time"); };

        if (SaveManager.Instance.IsFile($"DailyRewards"))
        {
            SaveManager.Load(ref DailyRewards, "DailyRewards");
        }

        onAutoSave += () => { SaveManager.SaveEnumerable(DailyRewards, "DailyRewards"); };

        for (int i = 0; i < 9; i++)
        {
            if (SaveManager.Instance.IsFile($"Deck_{i}"))
            {
                var load = Decks[i];
                SaveManager.LoadUnits(ref load, $"Deck_{i}");
            }
            SaveManager.SaveUnits(Decks[i], $"Deck_{i}");
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Time.timeScale = Time.timeScale == 1 ? 3 : 1;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Coin += 50;
            Jem += 50;
            PlayerLevel += 10;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!touch_up_effect)
                touch_up_effect = Instantiate(touch_up_effect_prefab);

            touch_up_effect.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touch_up_effect.Play();
        }
    }

    IEnumerator EAutoSave()
    {
        var wait = new WaitForSeconds(60);
        while (true)
        {
            on_auto_save();
            yield return wait;
        }
    }
    public IEnumerator EAppearUI(params Graphic[] graphics)
    {
        var wait = new WaitForSeconds(0.001f);
        while (graphics[0].color.a < 0.95f)
        {
            foreach (var graphic in graphics)
                graphic.color = Color.Lerp(graphic.color, new Color(graphic.color.r, graphic.color.g, graphic.color.b, 1), 0.1f);
            yield return wait;
        }
        foreach (var graphic in graphics) graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, 1);
        yield return null;
    }
    public IEnumerator EHideUI(params Graphic[] graphics)
    {
        var wait = new WaitForSeconds(0.001f);
        while (graphics[0].color.a > 0.05f)
        {
            foreach (var graphic in graphics)
                graphic.color = Color.Lerp(graphic.color, new Color(graphic.color.r, graphic.color.g, graphic.color.b, 0), 0.1f);
            yield return wait;
        }
        foreach (var graphic in graphics) graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, 0);
        UIManager.Instance.Fade.raycastTarget = false;
        yield return null;
    }

    private void OnApplicationQuit()
    {
        on_auto_save();
    }
}
