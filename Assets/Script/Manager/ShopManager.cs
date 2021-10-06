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

    [SerializeField] Image[] ButtonImgs = new Image[6];
    [SerializeField] Image[] BubbleMessage = new Image[3];

    [SerializeField] GameObject Unboxing;
    [SerializeField] RectTransform Box;
    [SerializeField] ParticleSystem RankParticle;
    [SerializeField] TextMeshProUGUI RewardCount;
    [SerializeField] RectTransform Card;
    [SerializeField] Button NextReward;

    TextMeshProUGUI[] Timer = new TextMeshProUGUI[3];

    readonly string FREE_SAPWN = "����̱� ����\n";

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
                Timer[i].text = $"���� �̱�!";
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(EUnboxing());
        }
    }

    IEnumerator EUnboxing()
    {
        RankParticle.Play();

        // ��ġ �Է�
        yield return new WaitForSeconds(1);

        RankParticle.Stop();

        yield return StartCoroutine(ERewardCountText());

        yield return StartCoroutine(EBoxMove());

        yield return StartCoroutine(EDrawing());

        // ���⼭ RewardCount�� 0�� �Ǹ� ������ ���� �����ִ� ȭ���� ���

        yield return StartCoroutine(EBoxMove(false));

        yield return StartCoroutine(ERewardCountText(false));

        yield return null;
    }

    IEnumerator ERewardCountText(bool hide = true)
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

    IEnumerator ECard(bool hide = true)
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

        yield return StartCoroutine(ECard(false));

        float rotate_force = 0;
        Vector2 card_pos = Card.anchoredPosition;
        Vector2 card_size = Card.sizeDelta;

        while (rotate_force < 2.8f)
        {
            rotate_force += Time.deltaTime;
            Card.anchoredPosition = Vector2.MoveTowards(Card.anchoredPosition, Vector2.up * ((Screen.height / 2) - (Card.sizeDelta.y / 2)), 1);
            Card.Rotate(Vector2.up * (rotate_force + 3));
            Card.sizeDelta = Vector2.Lerp(Card.sizeDelta, Vector2.zero, Time.deltaTime);
            yield return wait;
        }

        yield return StartCoroutine(ECard());

        Card.anchoredPosition = card_pos;
        Card.sizeDelta = card_size;

        yield return StartCoroutine(EShowResult());

        yield return null;
    }

    IEnumerator EShowResult()
    {
        print("Show something");
        yield return null;
    }
}
