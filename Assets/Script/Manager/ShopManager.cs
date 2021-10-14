using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public struct DateTimer
{
    public DateTime Date;
    public TimeSpan Time;
}

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; } = null;

    public List<Unit> SpawnUnits = new List<Unit>();

    [SerializeField] Image[] ButtonImgs = new Image[6];
    [SerializeField] Image[] BubbleMessage = new Image[3];

    [SerializeField] GameObject Unboxing;
    [SerializeField] RectTransform Box;
    [SerializeField] ParticleSystem RankParticle;
    [SerializeField] TextMeshProUGUI RewardCount;
    [SerializeField] RectTransform Card;
    [SerializeField] Button NextReward;
    [SerializeField] Image ShowUnitInCard;
    [SerializeField] RectTransform ShowAll;
    [SerializeField] GameObject ShowAllPrefab;

    TextMeshProUGUI[] Timer = new TextMeshProUGUI[3];

    Coroutine CUnBoxing;
    Coroutine CSkipUnBoxing;

    Button SkipBtn;

    readonly string FREE_SAPWN = "¹«·á»Ì±â ±îÁö\n";

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        for (int i = 0; i < 3; i++) Timer[i] = BubbleMessage[i].GetComponentInChildren<TextMeshProUGUI>();

        SkipBtn = Unboxing.transform.Find("Skip").GetComponent<Button>();
        SkipBtn.onClick.AddListener(() =>
        {
            if (CUnBoxing != null)
            {

                StopAllCoroutines();
                if (CSkipUnBoxing != null) StopCoroutine(CSkipUnBoxing);
                CSkipUnBoxing = StartCoroutine(ESkipUnboxing());
            }
        });
    }
    private void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            var times = GameManager.Instance.DateTimers[i];
            var left_time = times.Date + times.Time - DateTime.Now;
            if (left_time.TotalSeconds > 0)
            {
                Timer[i].text = $"{FREE_SAPWN}{left_time.Hours}:{left_time.Minutes}:{left_time.Seconds}";
            }
            else
            {
                Timer[i].text = $"¹«·á »Ì±â!";
            }
        }
    }

    public void AddCommon(int count)
    {
        var commons = UnitManager.Instance.Units.FindAll((o) => o.UnitType == UnitType.FRIEND && o.Info.Rank == Rank.COMMON);
        for (int i = 0; i < count; i++)
            SpawnUnits.Add(commons[UnityEngine.Random.Range(0, commons.Count)]);
    }
    public void AddRare(int count)
    {
        var rares = UnitManager.Instance.Units.FindAll((o) => o.UnitType == UnitType.FRIEND && o.Info.Rank == Rank.RARE);
        for (int i = 0; i < count; i++)
            SpawnUnits.Add(rares[UnityEngine.Random.Range(0, rares.Count)]);
    }
    public void AddEpic(int count)
    {
        var epics = UnitManager.Instance.Units.FindAll((o) => o.UnitType == UnitType.FRIEND && o.Info.Rank == Rank.EPIC);
        for (int i = 0; i < count; i++)
            SpawnUnits.Add(epics[UnityEngine.Random.Range(0, epics.Count)]);
    }
    public void AddLegend(int count)
    {
        var legends = UnitManager.Instance.Units.FindAll((o) => o.UnitType == UnitType.FRIEND && o.Info.Rank == Rank.LEGEND);
        for (int i = 0; i < count; i++)
            SpawnUnits.Add(legends[UnityEngine.Random.Range(0, legends.Count)]);
    }
    public void UnBoxing()
    {
        CUnBoxing = StartCoroutine(EUnboxing());
    }

    IEnumerator ESkipUnboxing()
    {
        yield return StartCoroutine(EBoxMove(false));

        yield return StartCoroutine(EShowAllResult());

        Image img = Unboxing.GetComponent<Image>();
        Color color = img.color;

        var wait = new WaitForSeconds(0.001f);

        while (img.color.a > 0.05f)
        {
            img.color = Color.Lerp(img.color, Color.clear, Time.deltaTime * 3);
            if (img.color.a < 0.3f) Unboxing.gameObject.SetActive(false);
            yield return wait;
        }
        img.color = color;

        SpawnUnits.Clear();
    }
    IEnumerator EUnboxing()
    {
        Unboxing.gameObject.SetActive(true);

        for (int i = SpawnUnits.Count - 1; i >= 0; i--)
        {
            RewardCount.text = $"{i + 1}";

            var particle_module = RankParticle.main;

            switch (SpawnUnits[i].Info.Rank)
            {
                case Rank.COMMON:
                    particle_module.startColor = Color.white;
                    break;
                case Rank.RARE:
                    particle_module.startColor = new Color(0, 1, 0.9023998f, 1);
                    break;
                case Rank.EPIC:
                    particle_module.startColor = new Color(0.7924528f, 0.4223923f, 0.6858099f, 1);
                    break;
                case Rank.LEGEND:
                    particle_module.startColor = new Color(0.9617409f, 0.9716981f, 0.2704254f, 1);
                    break;
            }

            RankParticle.Play();

            SkipBtn.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            SkipBtn.gameObject.SetActive(false);

            yield return StartCoroutine(EWaitClick());

            RankParticle.Stop();

            yield return StartCoroutine(EHideRewardCountText());

            yield return StartCoroutine(EBoxMove());

            yield return StartCoroutine(EHideCard(false));

            yield return StartCoroutine(EDrawing());

            yield return StartCoroutine(EHideCard());

            yield return StartCoroutine(EShowResult(SpawnUnits[i]));

            yield return StartCoroutine(EBoxMove(false));

            RewardCount.text = $"{i}";

            if (i == 0)
            {
                yield return StartCoroutine(EShowAllResult());

                Image img = Unboxing.GetComponent<Image>();
                Color color = img.color;

                var wait = new WaitForSeconds(0.001f);

                while (img.color.a > 0.05f)
                {
                    img.color = Color.Lerp(img.color, Color.clear, Time.deltaTime * 3);
                    if (img.color.a < 0.3f) Unboxing.gameObject.SetActive(false);
                    yield return wait;
                }
                img.color = color;
            }
            else yield return StartCoroutine(EHideRewardCountText(false));
        }

        SpawnUnits.Clear();

        yield return null;
    }
    IEnumerator EWaitClick()
    {
        var wait = new WaitForSeconds(0.001f);
        while (true)
        {
            if (Input.GetMouseButton(0)) yield break;
            yield return wait;
        }
    }
    IEnumerator EHideRewardCountText(bool hide = true)
    {
        var wait = new WaitForSeconds(0.001f);
        if (hide)
        {
            while (RewardCount.color.a > 0.05f)
            {
                RewardCount.color = Color.Lerp(RewardCount.color, Color.clear, Time.deltaTime * 3);
                yield return wait;
            }
            RewardCount.color = Color.clear;
        }
        else
        {
            while (RewardCount.color.a < 0.95f)
            {
                RewardCount.color = Color.Lerp(RewardCount.color, Color.white, Time.deltaTime * 3);
                yield return wait;
            }
            RewardCount.color = Color.white;
        }
        yield return null;
    }
    IEnumerator EBoxMove(bool down = true)
    {
        var wait = new WaitForSeconds(0.001f);
        Vector2 move_to = Vector2.zero;
        if (down)
        {
            move_to = new Vector2(0, Screen.height / 2 * -1 + (Box.sizeDelta.y / 2));
            while (Box.anchoredPosition.y > move_to.y + 0.05f)
            {
                Box.anchoredPosition = Vector2.Lerp(Box.anchoredPosition, move_to, Time.deltaTime * 3);
                yield return wait;
            }
        }
        else
        {
            while (Box.anchoredPosition.y < move_to.y - 0.05f)
            {
                Box.anchoredPosition = Vector2.Lerp(Box.anchoredPosition, move_to, Time.deltaTime * 3);
                yield return wait;
            }
        }

        Box.anchoredPosition = move_to;

        yield return null;
    }
    IEnumerator EHideCard(bool hide = true)
    {
        var wait = new WaitForSeconds(0.001f);
        var img = Card.GetComponent<Image>();
        if (hide)
        {
            while (img.color.a > 0.05f)
            {
                img.color = Color.Lerp(img.color, Color.clear, Time.deltaTime * 3);
                yield return wait;
            }
            img.color = Color.clear;
        }
        else
        {
            while (img.color.a < 0.95f)
            {
                img.color = Color.Lerp(img.color, Color.white, Time.deltaTime * 3);
                yield return wait;
            }
            img.color = Color.white;
        }
        yield return null;
    }
    IEnumerator EDrawing()
    {
        var wait = new WaitForSeconds(0.001f);

        float rotate_force = 0;
        Vector2 card_pos = Card.anchoredPosition;
        Vector2 card_size = Card.sizeDelta;

        while (Card.anchoredPosition.y < Screen.height / 2 - Card.sizeDelta.y)
        {
            rotate_force += Time.deltaTime;
            Card.anchoredPosition = Vector2.MoveTowards(Card.anchoredPosition, Vector2.up * ((Screen.height / 2) - (Card.sizeDelta.y / 2)), 500 * Time.deltaTime);
            Card.Rotate(Vector2.up * (rotate_force + 3));
            Card.sizeDelta = Vector2.Lerp(Card.sizeDelta, Vector2.zero, Time.deltaTime);
            yield return wait;
        }

        Card.anchoredPosition = card_pos;
        Card.sizeDelta = card_size;

        yield return null;
    }
    IEnumerator EShowResult(Unit unit)
    {
        var wait = new WaitForSeconds(0.001f);

        var rectTf = ShowUnitInCard.GetComponent<RectTransform>();

        Vector2 pos = rectTf.anchoredPosition;

        while (rectTf.anchoredPosition.y > 0.05f)
        {
            rectTf.anchoredPosition = Vector2.Lerp(rectTf.anchoredPosition, Vector2.right * rectTf.anchoredPosition, Time.deltaTime * 3);
            yield return wait;
        }

        yield return StartCoroutine(EWaitClick());

        var img = rectTf.GetChild(0).GetComponent<Image>();

        img.sprite = unit.Info.Icon;

        while (img.color.a < 0.95f)
        {
            img.color = Color.Lerp(img.color, Color.white, Time.deltaTime * 3);
            yield return wait;
        }
        img.color = Color.white;

        yield return StartCoroutine(EWaitClick());

        while (ShowUnitInCard.color.a > 0.05f)
        {
            ShowUnitInCard.color = Color.Lerp(ShowUnitInCard.color, Color.clear, Time.deltaTime * 3);
            img.color = Color.Lerp(img.color, Color.clear, Time.deltaTime * 3);
            yield return wait;
        }
        ShowUnitInCard.color = Color.clear;
        img.color = Color.clear;

        rectTf.anchoredPosition = pos;
        ShowUnitInCard.color = Color.white;
    }
    IEnumerator EShowAllResult()
    {
        var wait = new WaitForSeconds(0.001f);
        var img = ShowAll.GetComponent<Image>();

        ShowAll.gameObject.SetActive(true);

        while (img.color.a < 0.95f)
        {
            img.color = Color.Lerp(img.color, Color.white, Time.deltaTime * 3);
            yield return wait;
        }
        img.color = Color.white;

        List<GameObject> objs = new List<GameObject>();

        foreach (var unit in SpawnUnits)
        {
            var obj = Instantiate(ShowAllPrefab, ShowAll, false);
            var icon = obj.transform.GetChild(0).GetComponent<Image>();
            icon.sprite = unit.Info.Icon;
            objs.Add(obj);
            yield return new WaitForSeconds(0.5f);
        }

        yield return StartCoroutine(EWaitClick());

        ShowAll.gameObject.SetActive(false);

        for (int i = 0; i < objs.Count; i++)
            Destroy(objs[i]);
    }
}
