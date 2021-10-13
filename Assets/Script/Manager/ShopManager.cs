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

    TextMeshProUGUI[] Timer = new TextMeshProUGUI[3];

    readonly string FREE_SAPWN = "무료뽑기 까지\n";

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        for (int i = 0; i < 3; i++) Timer[i] = BubbleMessage[i].GetComponentInChildren<TextMeshProUGUI>();
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

    IEnumerator EUnboxing()
    {
        Unboxing.gameObject.SetActive(true);

        //for (int i = 0; i < SpawnUnits.Count; i++)
        foreach (var unit in SpawnUnits)
        {
            RankParticle.Play();

            yield return StartCoroutine(EWaitClick());

            RankParticle.Stop();

            yield return StartCoroutine(EHideRewardCountText());

            yield return StartCoroutine(EBoxMove());

            yield return StartCoroutine(EHideCard(false));

            yield return StartCoroutine(EDrawing());

            yield return StartCoroutine(EHideCard());

            yield return StartCoroutine(EShowResult(unit));

            // 여기서 RewardCount가 0이 되면 뽑은거 전부 보여주는 화면을 띄움

            yield return StartCoroutine(EBoxMove(false));

            yield return StartCoroutine(EHideRewardCountText(false));
        }

        //Unboxing.gameObject.SetActive(false);

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
            Card.anchoredPosition = Vector2.MoveTowards(Card.anchoredPosition, Vector2.up * ((Screen.height / 2) - (Card.sizeDelta.y / 2)), 1);
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
        print("Show something");

        yield return null;
    }
    IEnumerator EShowAllResult()
    {
        print("Show all");


        yield return null;
    }
}
