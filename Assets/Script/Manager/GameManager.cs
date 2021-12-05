using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using TMPro;

[Serializable]
public class DailyReward
{
    public StageInfoLinker.Reward_Kind kind;
    public int value;
    public bool gotten;
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

    private int playerLevel = 1;
    public int PlayerLevel
    {
        get => playerLevel;
        set
        {
            playerLevel = value;
            // TODO : 1레벨당 추가 효과
            if (playerLevel % 10 == 0)
            {
                // TODO : 10레벨 추가 효과, 아마 문서로 정리되야 할듯
            }
        }
    }
    public int player_exp = 0;
    public int need_exp = 100;
    public int Coin = 0;
    public int Jem = 0;
    public int Stemina = 20;
    public int MaxStemina = 20;
    public int UnBoxingCount = 11;

    public bool EnteredDeckView = false;

    public readonly int theme_count = 3;

    public List<DateTimer> DateTimers = new List<DateTimer>(3);

    public DateTimer Daily = new DateTimer { Time = new TimeSpan(24, 0, 0) };

    private int daily_days = 0;
    public int DailyDays
    {
        get { return daily_days; }
        set
        {
            daily_days = value;
            daily_days %= 28;
        }
    }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        StartCoroutine(EAutoSave());

        var resource = SaveManager.Load<int>("ResourceData");

        if (resource != null)
        {
            Coin = resource.ElementAt(0);
            Jem = resource.ElementAt(1);
            Stemina = resource.ElementAt(2);
            MaxStemina = resource.ElementAt(3);
        }

        onAutoSave += () => { SaveManager.Save(new List<int>() { Coin, Jem, Stemina, MaxStemina }, "ResourceData"); };

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

        onAutoSave += () => { SaveManager.Save(DateTimers.Select((o) => o.Date.ToString()), "DateTimers_Date"); };
        onAutoSave += () => { SaveManager.Save(DateTimers.Select((o) => o.Time.ToString()), "DateTimers_Time"); };

        if (SaveManager.Instance.IsFile($"DailyRewards"))
        {
            SaveManager.Load(ref DailyRewards, "DailyRewards");
        }
        else
        {
            for (int i = 0; i < 28; i++)
            {
                int div = i / 7;
                switch (div)
                {
                    case 0:
                        DailyRewards.Add(new DailyReward { gotten = false, kind = StageInfoLinker.Reward_Kind.Stemina, value = 1 });
                        break;
                    case 1:
                        DailyRewards.Add(new DailyReward { gotten = false, kind = StageInfoLinker.Reward_Kind.Jem, value = 2 });
                        break;
                    case 2:
                        DailyRewards.Add(new DailyReward { gotten = false, kind = StageInfoLinker.Reward_Kind.Coin, value = 3 });
                        break;
                    case 3:
                        DailyRewards.Add(new DailyReward { gotten = false, kind = StageInfoLinker.Reward_Kind.Jem, value = 4 });
                        break;
                }
            }
        }
        onAutoSave += () => { SaveManager.Save(DailyRewards, "DailyRewards"); };

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
                graphic.color = Color.Lerp(graphic.color, new Color(graphic.color.r, graphic.color.g, graphic.color.b, 1), Time.deltaTime * 3);
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
                graphic.color = Color.Lerp(graphic.color, new Color(graphic.color.r, graphic.color.g, graphic.color.b, 0), Time.deltaTime * 3);
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
