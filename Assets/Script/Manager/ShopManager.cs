#define NO_OVERLAP_IN_SPAWN

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using UnityEngine.Events;

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

    public Dictionary<Unit, int> SpawnUnits = new Dictionary<Unit, int>();
    public TextMeshProUGUI txtOneCost;
    public TextMeshProUGUI txtElevenCost;
    public TextMeshProUGUI[] rankTexts = new TextMeshProUGUI[2];
    public RectTransform DontBuyWindow;
    public RectTransform BuyWindow;
    public Image[] imgResourceIcon;
    public Button btnOneBuy;
    public Button btnElevenBuy;
    public Image Upper;
    public Image Lower;
    public Image imgBuyBox;
    public Image imgDropBox;
    public List<UpperLowerSprite> UpperLowers;
    public List<Sprite> BoxSprites;

    public UnityAction[] actions = new UnityAction[3];

    [SerializeField] Image[] BubbleMessage = new Image[3];
    [SerializeField] Image Fade;
    [SerializeField] ParticleSystem ps;
    [SerializeField] RectTransform AllResultParent;
    [SerializeField] RectTransform ShowCard;
    [SerializeField] GameObject ResultPrefab;
    [SerializeField] Button Skip;

    [SerializeField] ParticleSystem drop_box_particle;

    TextMeshProUGUI[] Timer = new TextMeshProUGUI[3];

    readonly string FREE_SAPWN = "무료뽑기 까지\n";

    public int selected_box_index = 0;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        for (int i = 0; i < 3; i++) Timer[i] = BubbleMessage[i].GetComponentInChildren<TextMeshProUGUI>();
        Skip.onClick.AddListener(() =>
        {
            StopAllCoroutines();
            ShowCard.gameObject.SetActive(false);
            StartCoroutine(UIManager.Instance.EColoringUI(Lower, Color.clear, 10));
            StartCoroutine(UIManager.Instance.EColoringUI(Upper, Color.clear, 10));
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

        if (Input.GetKeyDown(KeyCode.Alpha1))
            actions[0]();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            actions[1]();
        if (Input.GetKeyDown(KeyCode.Alpha3))
            actions[2]();
        if (Input.GetKeyDown(KeyCode.Return))
            actions[selected_box_index]();
    }

    public void AddCommon(int count)
    {
        var commons = UnitManager.Instance.Units.FindAll((o) => o.UnitType == UnitType.FRIEND && o.Info.Rank == Rank.COMMON && !o.IsLock);

#if NO_OVERLAP_IN_SPAWN
        List<int> randoms = new List<int>();

        for (int i = 0; i < commons.Count; i++)
            randoms.Add(i);
#endif

        for (int i = 0; i < count; i++)
        {
#if NO_OVERLAP_IN_SPAWN
            int rand = UnityEngine.Random.Range(0, randoms.Count);
            var unit = commons[rand];
            randoms.Remove(rand);
#else
            var unit = commons[UnityEngine.Random.Range(0, commons.Count)];
#endif
            if (SpawnUnits.ContainsKey(unit))
                SpawnUnits[unit]++;
            else
                SpawnUnits.Add(unit, 1);
        }
    }
    public void AddRare(int count)
    {
        var rares = UnitManager.Instance.Units.FindAll((o) => o.UnitType == UnitType.FRIEND && o.Info.Rank == Rank.RARE && !o.IsLock);

#if NO_OVERLAP_IN_SPAWN
        List<int> randoms = new List<int>();

        for (int i = 0; i < rares.Count; i++)
            randoms.Add(i);
#endif

        for (int i = 0; i < count; i++)
        {
#if NO_OVERLAP_IN_SPAWN
            int rand = UnityEngine.Random.Range(0, randoms.Count);
            var unit = rares[rand];
            randoms.Remove(rand);
#else
            var unit = rares[UnityEngine.Random.Range(0, rares.Count)];
#endif
            if (SpawnUnits.ContainsKey(unit))
                SpawnUnits[unit]++;
            else
                SpawnUnits.Add(unit, 1);
        }
    }
    public void AddEpic(int count)
    {
        var epics = UnitManager.Instance.Units.FindAll((o) => o.UnitType == UnitType.FRIEND && o.Info.Rank == Rank.EPIC && !o.IsLock);

#if NO_OVERLAP_IN_SPAWN
        List<int> randoms = new List<int>();

        for (int i = 0; i < epics.Count; i++)
            randoms.Add(i);
#endif

        for (int i = 0; i < count; i++)
        {
#if NO_OVERLAP_IN_SPAWN
            int rand = UnityEngine.Random.Range(0, randoms.Count);
            var unit = epics[rand];
            randoms.Remove(rand);
#else
            var unit = epics[UnityEngine.Random.Range(0, epics.Count)];
#endif

            if (SpawnUnits.ContainsKey(unit))
                SpawnUnits[unit]++;
            else
                SpawnUnits.Add(unit, 1);
        }
    }
    public void AddLegend(int count)
    {
        var legends = UnitManager.Instance.Units.FindAll((o) => o.UnitType == UnitType.FRIEND && o.Info.Rank == Rank.LEGEND && !o.IsLock);

#if NO_OVERLAP_IN_SPAWN
        List<int> randoms = new List<int>();

        for (int i = 0; i < legends.Count; i++)
            randoms.Add(i);
#endif

        for (int i = 0; i < count; i++)
        {
#if NO_OVERLAP_IN_SPAWN
            int rand = UnityEngine.Random.Range(0, randoms.Count);
            var unit = legends[rand];
            randoms.Remove(rand);
#else
            var unit = legends[UnityEngine.Random.Range(0, legends.Count)];
#endif
            if (SpawnUnits.ContainsKey(unit))
                SpawnUnits[unit]++;
            else
                SpawnUnits.Add(unit, 1);
        }
    }

    Coroutine drop_box = null;
    public void DropBox()
    {
        if (drop_box != null) StopCoroutine(drop_box);
        drop_box = StartCoroutine(_DropBox());
    }
    IEnumerator _DropBox()
    {
        imgDropBox.rectTransform.anchoredPosition = new Vector2(-650, 1000);

        var drop_box_animator = imgDropBox.GetComponent<Animator>();
        drop_box_animator.enabled = false;

        yield return StartCoroutine(UIManager.Instance.EMovingUI(imgDropBox, new Vector2(-650, -260), 3000));
        drop_box_particle.Play();

        WaitForSeconds second = new WaitForSeconds(0.01f);

        Vector2 target_scale = new Vector2(1.2f, 0.8f);
        while (Vector2.Distance(imgDropBox.rectTransform.localScale, target_scale) > 0.01f)
        {
            imgDropBox.rectTransform.localScale = Vector2.Lerp(imgDropBox.rectTransform.localScale, target_scale, 0.25f);
            Debug.Log("1");
            yield return second;
        }
        imgDropBox.rectTransform.localScale = target_scale;

        while (Vector2.Distance(imgDropBox.rectTransform.localScale, Vector2.one) > 0.01f)
        {
            imgDropBox.rectTransform.localScale = Vector2.Lerp(imgDropBox.rectTransform.localScale, Vector2.one, 0.25f);
            Debug.Log("2");
            yield return second;
        }
        imgDropBox.rectTransform.localScale = Vector2.one;

        drop_box_animator.enabled = true;
        drop_box = null;
    }

    public void UnboxCancel()
    {
        StopAllCoroutines();
        SpawnUnits.Clear();
        BuyWindow.GetComponentInChildren<PopupWindow>().ClosePopupWindow();

        var drop_box_btn = imgDropBox.GetComponent<Button>();
        drop_box_btn.onClick.RemoveAllListeners();
        drop_box_btn.onClick.AddListener(actions[selected_box_index]);

        imgDropBox.rectTransform.anchoredPosition = new Vector2(-650, -260);
        imgDropBox.rectTransform.localScale = Vector2.one;
    }

    public void Unboxing(bool one)
    {
        BuyWindow.GetComponentInChildren<PopupWindow>().ClosePopupWindow();
        GameManager.Instance.Coin -= one ? BuyBreadLinker.Coin_one : BuyBreadLinker.Coin_eleven;
        GameManager.Instance.Jem -= one ? BuyBreadLinker.Jem_one : BuyBreadLinker.Jem_eleven;
        StartCoroutine(EUnBoxing());
    }

    public void ResetUnboxing()
    {
        ps.Stop();
        UnboxCancel();

        Fade.color = Color.clear;
        Skip.gameObject.SetActive(false);
        Lower.color = Upper.color = Color.clear;
        Lower.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -124);
        Upper.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 95);
    }

    IEnumerator EUnBoxing()
    {
        yield return StartCoroutine(UIManager.Instance.EColoringUI(Fade, Color.black * 0.6f, 3));

        Fade.raycastTarget = true;

        Skip.gameObject.SetActive(true);

        var ps_main = ps.main;

        ps.Stop();

        // 상자 스프라이트 바꿔줘야함 일단 기본

        // 상자 투명 -> 불 투명
        StartCoroutine(UIManager.Instance.EColoringUI(Lower, Color.white, 3));
        yield return StartCoroutine(UIManager.Instance.EColoringUI(Upper, Color.white, 3));

        // 상자 뚜껑 열리기
        var upper_pos = Upper.GetComponent<RectTransform>();
        SoundManager.Instance.Play("SFX/Spawn Bread/Spawn Bread", SoundType.EFFECT);
        yield return StartCoroutine(UIManager.Instance.EMovingUI(Upper, (Screen.height / 2 + upper_pos.sizeDelta.y) * Vector2.up, 5000));

        bool first = true;
        foreach (var spawn_unit in SpawnUnits)
        {
            switch (spawn_unit.Key.Info.Rank)
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

            if (first)
            {
                ps.Play();
                yield return StartCoroutine(EClick());
                ps.Stop();

                first = false;
            }


            /*            // 나온 카드 보여주기 SpawnUnits[i]
                        var imgShow = ShowCard.GetChild(0).GetComponent<Image>();
                        imgShow.sprite = SpawnUnits[i].Info.Icon;
                        UIManager.Instance.FixSizeToRatio(imgShow, ShowCard.sizeDelta.x - 20);
                        ShowCard.gameObject.SetActive(true);

                        // yield return StartCoroutine(EClick());

                        ShowCard.gameObject.SetActive(false);*/
        }

        Skip.gameObject.SetActive(false);

        // 상자 불 투명 -> 투명
        StartCoroutine(UIManager.Instance.EColoringUI(Lower, Color.clear, 3));
        yield return StartCoroutine(UIManager.Instance.EColoringUI(Upper, Color.clear, 3));

        yield return StartCoroutine(ESkipAndShowAllResult());

        yield return null;
    }

    IEnumerator ESkipAndShowAllResult()
    {
        ps.Stop();

        var wait = new WaitForSeconds(0.1f);

        List<GameObject> objs = new List<GameObject>();

        foreach (var spawn_unit in SpawnUnits)
        {
            // 등급에 따라 뒤에서 빛나오게 하기
            GameObject obj = Instantiate(ResultPrefab, AllResultParent, false);
            Image img = obj.transform.GetChild(0).GetComponent<Image>();
            var count_text = obj.GetComponentInChildren<TextMeshProUGUI>();
            var GLG = AllResultParent.GetComponent<GridLayoutGroup>();
            var newIcon = obj.transform.GetChild(2).gameObject;

            img.sprite = spawn_unit.Key.Info.Icon;
            count_text.text = spawn_unit.Value.ToString();
            newIcon.SetActive(!spawn_unit.Key.Info.Gotten);


            UIManager.Instance.FixSizeToRatio(img, GLG.cellSize.x - 20);
            spawn_unit.Key.Info.Count++;

            objs.Add(obj);

            SoundManager.Instance.Play("SFX/Spawn Bread/Each Bread", SoundType.EFFECT);

            yield return wait;
        }

        /*        for (int i = SpawnUnits.Count - 1; i >= 0; i--)
                {
                    // 등급에 따라 뒤에서 빛나오게 하기
                    GameObject obj = Instantiate(ResultPrefab, AllResultParent, false);
                    Image img = obj.transform.GetChild(0).GetComponent<Image>();
                    var GLG = AllResultParent.GetComponent<GridLayoutGroup>();
                    img.sprite = SpawnUnits[i].Info.Icon;
                    UIManager.Instance.FixSizeToRatio(img, GLG.cellSize.x - 20);
                    SpawnUnits[i].Info.Count++;

                    objs.Add(obj);

                    yield return wait;
                }*/

        yield return StartCoroutine(EClick());

        for (int i = 0; i < objs.Count; i++)
            Destroy(objs[i]);

        yield return StartCoroutine(UIManager.Instance.EColoringUI(Fade, Color.clear, 3));

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
