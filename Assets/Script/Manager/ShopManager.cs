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
    public TextMeshProUGUI[] txtCost;
    public RectTransform DontBuyWindow;
    public RectTransform BuyWindow;
    public Image imgResourceIcon;
    public Button btnBuy;
    public Image Upper;
    public Image Lower;
    public Image imgBuyBox;
    public Image imgDropBox;
    public List<UpperLowerSprite> UpperLowers;
    public List<Sprite> BoxSprites;

    [SerializeField] Image[] ButtonImgs = new Image[6];
    [SerializeField] Image[] BubbleMessage = new Image[3];
    [SerializeField] Image Fade;
    [SerializeField] ParticleSystem ps;
    [SerializeField] RectTransform AllResultParent;
    [SerializeField] RectTransform ShowCard;
    [SerializeField] GameObject ResultPrefab;
    [SerializeField] Button Skip;


    TextMeshProUGUI[] Timer = new TextMeshProUGUI[3];

    readonly string FREE_SAPWN = "����̱� ����\n";

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
                Timer[i].text = $"���� �̱�!";
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

    public void UnboxCancel()
    {
        StopAllCoroutines();
        SpawnUnits.Clear();
        BuyWindow.gameObject.SetActive(false);
        imgDropBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(-600, 1000);
    }

    public void Unboxing()
    {
        BuyWindow.gameObject.SetActive(false);
        GameManager.Instance.Coin -= SpawnBtnLinker.Coin;
        GameManager.Instance.Jem -= SpawnBtnLinker.Jem;
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
        yield return StartCoroutine(UIManager.Instance.EColoringUI(Fade, Color.black * 0.6588235f, 3));

        Fade.raycastTarget = true;

        Skip.gameObject.SetActive(true);

        var ps_main = ps.main;

        ps.Stop();

        // ���� ��������Ʈ �ٲ������ �ϴ� �⺻

        // ���� ���� -> �� ����
        StartCoroutine(UIManager.Instance.EColoringUI(Lower, Color.white, 3));
        yield return StartCoroutine(UIManager.Instance.EColoringUI(Upper, Color.white, 3));

        // ���� �Ѳ� ������
        var upper_pos = Upper.GetComponent<RectTransform>();
        yield return StartCoroutine(UIManager.Instance.EMovingUI(Upper, (Screen.height / 2 + upper_pos.sizeDelta.y) * Vector2.up, 5000));

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

            // ���� ī�� �����ֱ� SpawnUnits[i]
            var imgShow = ShowCard.GetChild(0).GetComponent<Image>();
            imgShow.sprite = SpawnUnits[i].Info.Icon;
            UIManager.Instance.FixSizeToRatio(imgShow, ShowCard.sizeDelta.x - 20);
            ShowCard.gameObject.SetActive(true);

            yield return StartCoroutine(EClick());

            ShowCard.gameObject.SetActive(false);
        }

        Skip.gameObject.SetActive(false);

        // ���� �� ���� -> ����
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

        for (int i = SpawnUnits.Count - 1; i >= 0; i--)
        {
            // ��޿� ���� �ڿ��� �������� �ϱ�
            GameObject obj = Instantiate(ResultPrefab, AllResultParent, false);
            Image img = obj.transform.GetChild(0).GetComponent<Image>();
            var GLG = AllResultParent.GetComponent<GridLayoutGroup>();
            img.sprite = SpawnUnits[i].Info.Icon;
            UIManager.Instance.FixSizeToRatio(img, GLG.cellSize.x - 20);
            SpawnUnits[i].Info.Count++;

            objs.Add(obj);

            yield return wait;
        }

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
