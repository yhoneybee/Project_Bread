using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public struct DoActionForPersent
{
    private int persent;
    public int Persent
    {
        get { return persent; }
        set
        {
            persent = value;
            persent = Mathf.Min(Mathf.Max(persent, 1), 100);
        }
    }
    public System.Action Action;
}

public class ButtonActions : MonoBehaviour
{
    public static ButtonActions Instance { get; private set; } = null;

    private void Awake()
    {
        Instance = this;
    }

    public bool CheckReEntering(string name)
    {
        string scene = SceneManager.GetActiveScene().name;
        return name == scene;
    }
    public void GoHome()
    {
        SceneManager.LoadScene("B - Main");
    }
    public void ChangeScene(string name)
    {
        if (name == "E - 01 DeckView")
            GameManager.Instance.EnteredDeckView = true;

        if (GameManager.Instance.EnteredDeckView && name == "B - Main")
        {
            GameManager.Instance.EnteredDeckView = false;
            ChangeScene("E - 01 DeckView");
            return;
        }

        SceneManager.LoadScene(name);
    }
    /// <summary>
    /// �׸� ���� �� ȣ������� �ϴ� �Լ�
    /// </summary>
    /// <param name="theme_number">�׸� ��ȣ (ex : 1)</param>
    public void SetThemeNumber(int theme_number)
    {
        string[] theme_names = { "밝은 오븐", "넓은 등판", "음침한 숲" };

        // �׸� ��ȣ ���� �� �׸� �̸� �ٲ���
        StageInfo.theme_number = theme_number;
        StageInfo.theme_name = theme_names[theme_number - 1];
    }
    public void ChangeDeck(int index)
    {
        GameManager.Instance.Index = index;
    }
    public void ExceptUnit()
    {
        GameManager.Select[GameManager.SelectSlotIdx] = null;
        ChangeScene("C - 03 DeckSelect");
    }
    public void ChangeAnimState(int index)
    {
        UIManager.Instance.AnimState = (AnimState)index;
    }
    public void DoForPersent(params DoActionForPersent[] persent_actions)
    {
        int persent = Random.Range(1, 101);
        for (int i = 0; i < persent_actions.Length; i++)
        {
            if (persent_actions[i].Persent <= persent)
                persent_actions[i].Action();
        }
    }
    public void UnBoxingOne(int rank)
    {
        switch (rank)
        {
            case 0: // common
                DoForPersent(
                    new DoActionForPersent { Persent = 90, Action = () => { ShopManager.Instance.AddRare(1); } },
                    new DoActionForPersent { Persent = 10, Action = () => { ShopManager.Instance.AddCommon(1); } }
                    );
                break;
            case 1: // rare
                DoForPersent(
                    new DoActionForPersent { Persent = 90, Action = () => { ShopManager.Instance.AddEpic(1); } },
                    new DoActionForPersent { Persent = 10, Action = () => { ShopManager.Instance.AddRare(1); } }
                    );
                break;
            case 2: // epic
                DoForPersent(
                    new DoActionForPersent { Persent = 90, Action = () => { ShopManager.Instance.AddLegend(1); } },
                    new DoActionForPersent { Persent = 10, Action = () => { ShopManager.Instance.AddEpic(1); } }
                    );
                break;
            case 3: // legend
                ShopManager.Instance.AddLegend(1);
                break;
        }

        ShopManager.Instance.UnBoxing();
    }
    public void UnBoxingTen(int rank)
    {
        int persent_max = GameManager.Instance.UnBoxingCount + 1;
        int soso = Random.Range(persent_max / 2, persent_max);
        soso += Random.Range(0, persent_max - soso);
        int good = Random.Range(0, persent_max - soso);
        int great = Random.Range(0, persent_max - soso - good);
        soso += GameManager.Instance.UnBoxingCount - good - great - soso;

        switch (rank)
        {
            case 0: // common
                break;
            case 1: // rare
                if (Random.Range(1, 101) >= 50)
                {
                    --soso;
                    ++good;
                }
                break;
            case 2: // epic
                --soso;
                ++good;
                break;
            case 3: // legend
                --good;
                ++great;
                break;
        }

        ShopManager.Instance.AddCommon(soso);
        ShopManager.Instance.AddRare(good);
        ShopManager.Instance.AddEpic(great);

        ShopManager.Instance.UnBoxing();
    }
    public void AppearAndHideForPivot(RectTransform RT)
    {
        StartCoroutine(EAppearAndHideForPivot(RT));
    }
    IEnumerator EAppearAndHideForPivot(RectTransform RT)
    {
        var arrow = RT.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>();
        if (RT.pivot.x == 0.9f)
        {
            while (RT.pivot.x > 0.005f)
            {
                RT.pivot = Vector2.Lerp(RT.pivot, new Vector2(0, RT.pivot.y), Time.deltaTime * 3);
                if (RT.pivot.x < 0.3f) arrow.sprite = UIManager.Instance.ArrowSwitchSprite.BSprite;
                yield return new WaitForSeconds(0.001f);
            }
            RT.pivot = new Vector2(0, RT.pivot.y);
        }
        else if (RT.pivot.x == 0)
        {
            while (RT.pivot.x < 0.895f)
            {
                RT.pivot = Vector2.Lerp(RT.pivot, new Vector2(1, RT.pivot.y), Time.deltaTime * 3);
                if (RT.pivot.x > 0.6f) arrow.sprite = UIManager.Instance.ArrowSwitchSprite.ASprite;
                yield return new WaitForSeconds(0.001f);
            }
            RT.pivot = new Vector2(0.9f, RT.pivot.y);
        }

        yield return null;
    }
}