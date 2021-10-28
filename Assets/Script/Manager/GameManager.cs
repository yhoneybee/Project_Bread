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

    public int player_level = 1;
    public int Coin = 0;
    public int Jem = 0;
    public int Stemina = 0;
    public int MaxStemina = 20;
    public int UnBoxingCount = 11;

    public bool EnteredDeckView = false;

    public readonly int theme_count = 3;

    public List<DateTimer> DateTimers = new List<DateTimer>();

    public DateTimer Daily = new DateTimer { Time = new TimeSpan(24, 0, 0) };
    public int daily_days = 0;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        if (SaveManager.Instance.IsFile($"DateTimers"))
        {
            SaveManager.Load(ref DateTimers, "DateTimers");
        }
        else
        {
            for (int i = 0; i < 3; i++) DateTimers.Add(new DateTimer { Date = DateTime.Now, Time = new TimeSpan(0, 30 * (i + 1), 0) });
            SaveManager.Save(DateTimers, "DateTimers");
        }

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
            SaveManager.Save(DailyRewards, "DailyRewards");
        }

        for (int i = 0; i < 9; i++)
        {
            if (SaveManager.Instance.IsFile($"Deck_{i}"))
            {
                var loads = SaveManager.Load<string>($"Deck_{i}");

                for (int j = 0; j < loads.Count(); j++)
                {
                    var find = UnitManager.Instance.Units.Find((o) => loads.ElementAt(j) == o.Info.Name);
                    if (find != null) Decks[i][j] = find;
                }
            }
            else
            {
                SaveManager.Save(Decks[i].Select((o) => { if (o != null) { return o.Info.Name; } return "___"; }), $"Deck_{i}");
            }
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
            player_level += 10;
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
        yield return null;
    }

    private void OnApplicationQuit()
    {
        for (int i = 0; i < 9; i++)
        {
            SaveManager.Save(Decks[i].Select((o) => { if (o != null) { return o.Info.Name; } return "___"; }), $"Deck_{i}");
        }
    }
}
