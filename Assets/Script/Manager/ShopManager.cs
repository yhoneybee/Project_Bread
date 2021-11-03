using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

[Serializable]
public class DateTimer
{
    public DateTime Date;
    public TimeSpan Time;
}

[Serializable]
public struct UpperLowerSprite
{
    public Sprite Upper;
    public Sprite Lower;
}

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; } = null;

    public List<Unit> SpawnUnits = new List<Unit>();

    [SerializeField] Image[] ButtonImgs = new Image[6];
    [SerializeField] Image[] BubbleMessage = new Image[3];

    [SerializeField] Image Fade;
    [SerializeField] ParticleSystem ps;
    [SerializeField] RectTransform AllResultParent;
    [SerializeField] GameObject ResultPrefab;
    [SerializeField] Button Skip;
    [SerializeField] Image Upper;
    [SerializeField] Image Lower;

    [SerializeField] List<UpperLowerSprite> UpperLowers;

    TextMeshProUGUI[] Timer = new TextMeshProUGUI[3];

    readonly string FREE_SAPWN = "무료뽑기 까지\n";

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        for (int i = 0; i < 3; i++) Timer[i] = BubbleMessage[i].GetComponentInChildren<TextMeshProUGUI>();
        Skip.onClick.AddListener(() =>
        {
            ps.Stop();
            StartCoroutine(EColoringUI(Lower, Color.clear, 10));
            StartCoroutine(EColoringUI(Upper, Color.clear, 10));
            StartCoroutine(ESkipAndShowAllResult());
            Skip.gameObject.SetActive(false);
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
                Timer[i].text = $"무료 뽑기!";
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

    public void Unboxing()
    {
        StartCoroutine(EUnBoxing());
    }

    public void ResetUnboxing()
    {
        StopAllCoroutines();

        Fade.color = Color.clear;
        Skip.gameObject.SetActive(false);
        Lower.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -124);
        Upper.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 95);
    }

    IEnumerator EUnBoxing()
    {
        yield return StartCoroutine(EColoringUI(Fade, Color.black * 0.6588235f, 3));

        Fade.raycastTarget = true;

        Skip.gameObject.SetActive(true);

        var ps_main = ps.main;

        ps.Stop();

        // 상자 스프라이트 바꿔줘야함 일단 기본

        // 상자 투명 -> 불 투명
        StartCoroutine(EColoringUI(Lower, Color.white, 3));
        yield return StartCoroutine(EColoringUI(Upper, Color.white, 3));

        // 상자 뚜껑 열리기
        var upper_pos = Upper.GetComponent<RectTransform>();
        yield return StartCoroutine(EMovingUI(Upper, (Screen.height / 2 + upper_pos.sizeDelta.y) * Vector2.up, 5000));

        for (int i = SpawnUnits.Count - 1; i >= 0; i--)
        {
            print($"IDX : {i}");
            switch (SpawnUnits[i].Info.Rank)
            {
                case Rank.COMMON:
                    ps_main.startColor = Color.white;
                    break;
                case Rank.RARE:
                    ps_main.startColor = new Color(0, 1, 0.9023998f, 1);
                    break;
                case Rank.EPIC:
                    ps_main.startColor = new Color(0.7924528f, 0.4223923f, 0.6858099f, 1);
                    break;
                case Rank.LEGEND:
                    ps_main.startColor = new Color(0.9617409f, 0.9716981f, 0.2704254f, 1);
                    break;
            }

            ps.Play();

            yield return StartCoroutine(EClick());

            ps.Stop();

            // 나온 카드 보여주기
        }

        Skip.gameObject.SetActive(false);

        // 상자 불 투명 -> 투명
        StartCoroutine(EColoringUI(Lower, Color.clear, 3));
        yield return StartCoroutine(EColoringUI(Upper, Color.clear, 3));

        yield return StartCoroutine(ESkipAndShowAllResult());

        yield return null;
    }

    IEnumerator EMovingUI<T>(T ui, Vector2 change_pos, float move_speed, bool isLerp = false)
    where T : Graphic
    {
        var wait = new WaitForSeconds(0.0001f);
        var uiRTf = ui.GetComponent<RectTransform>();

        while (Vector2.Distance(uiRTf.anchoredPosition, change_pos) > 0.1f)
        {
            if (isLerp) uiRTf.anchoredPosition = Vector2.Lerp(uiRTf.anchoredPosition, change_pos, Time.deltaTime * move_speed);
            else uiRTf.anchoredPosition = Vector2.MoveTowards(uiRTf.anchoredPosition, change_pos, Time.deltaTime * move_speed);
            yield return wait;
        }
        uiRTf.anchoredPosition = change_pos;

        yield return null;
    }

    IEnumerator EColoringUI<T>(T ui, Color change_color, float change_speed)
    where T : Graphic
    {
        var wait = new WaitForSeconds(0.0001f);

        while (Mathf.Abs(ui.color.r - change_color.r) +
            Mathf.Abs(ui.color.g - change_color.g) +
            Mathf.Abs(ui.color.b - change_color.b) +
            Mathf.Abs(ui.color.a - change_color.a) > 0.005f)
        {
            ui.color = Color.Lerp(ui.color, change_color, Time.deltaTime * change_speed);
            yield return wait;
        }
        ui.color = change_color;

        yield return null;
    }

    IEnumerator ESkipAndShowAllResult()
    {
        var wait = new WaitForSeconds(0.5f);

        List<GameObject> objs = new List<GameObject>();

        for (int i = SpawnUnits.Count - 1; i >= 0; i--)
        {
            // 등급에 따라 뒤에서 빛나오게 하기
            GameObject obj = Instantiate(ResultPrefab, AllResultParent, false);
            Image img = obj.transform.GetChild(0).GetComponent<Image>();
            var GLG = AllResultParent.GetComponent<GridLayoutGroup>();
            img.sprite = SpawnUnits[i].Info.Icon;
            UIManager.Instance.FixSizeToRatio(img, GLG.cellSize.x - 20);

            objs.Add(obj);

            yield return wait;
        }

        yield return StartCoroutine(EClick());

        for (int i = 0; i < objs.Count; i++)
            Destroy(objs[i]);

        yield return StartCoroutine(EColoringUI(Fade, Color.clear, 3));

        Fade.raycastTarget = false;

        ResetUnboxing();
        SpawnUnits.Clear();

        yield return null;
    }

    IEnumerator EClick()
    {
        var wait = new WaitForSeconds(0.001f);

        while (true)
        {
            if (Input.GetMouseButtonDown(0)) break;
            yield return wait;
        }
        yield return null;
    }
}
