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

    public bool CheckReEntering(string name) => name == SceneManager.GetActiveScene().name;

    public void GoHome()
    {
        SceneManager.LoadScene("B-Main");
    }

    private Coroutine CChangeScene;
    public void ChangeScene(string name)
    {
        //StopAllCoroutines();
        //UIManager.Instance.Fade.color = Color.clear;
        if (CChangeScene == null)
            CChangeScene = StartCoroutine(EChangeScene(name));
    }

    IEnumerator EChangeScene(string name)
    {
        var fade_img = UIManager.Instance.Fade;
        var wait = new WaitForSeconds(0.001f);

        while (fade_img.color.a < 0.95)
        {
            fade_img.color = Color.Lerp(fade_img.color, Color.black, Time.deltaTime * 3);
            yield return wait;
        }
        fade_img.color = Color.black;

        if (CheckReEntering("E-01_DeckView")) GameManager.Instance.EnteredDeckView = true;
        else if (CheckReEntering("D-01_StageSelect")) GameManager.Instance.EnteredDeckView = false;

        if (GameManager.Instance.EnteredDeckView && name == "B-Main")
        {
            GameManager.Instance.EnteredDeckView = false;
            ChangeScene("E-01_DeckView");
            yield break;
        }

        SceneManager.LoadScene(name);

        yield return null;
    }

    public void SetThemeNumber(int theme_number)
    {
        string[] theme_names = { "밝은 오븐", "넓은 등판", "음침한 숲", "???", "업데이트 예정" };

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
        ChangeScene("C-03_DeckSelect");
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

        ShopManager.Instance.BuyWindow.gameObject.SetActive(true);
        //ShopManager.Instance.Unboxing();
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

        ShopManager.Instance.BuyWindow.gameObject.SetActive(true);
        //ShopManager.Instance.Unboxing();
    }

    public void AppearAndHideForPivot(RectTransform RT)
    {
        StartCoroutine(EAppearAndHideForPivot(RT));
    }

    public void GameStart()
    {
        if (GameManager.Instance.Stemina >= 3)
        {
            GameManager.Instance.Stemina -= 3;
            ChangeScene("F-01_Ingame");
        }
        else
        {
            // TODO : 플레이 못함
            StopAllCoroutines();
            UIManager.Instance.txtNoStemina.color = Color.clear;
            StartCoroutine(ENoStemina());
        }
    }

    IEnumerator ENoStemina()
    {
        yield return StartCoroutine(UIManager.Instance.EColoringUI(UIManager.Instance.txtNoStemina, Color.white, 3));

        yield return new WaitForSeconds(1);

        StartCoroutine(UIManager.Instance.EColoringUI(UIManager.Instance.txtNoStemina, Color.clear, 3));

        yield return null;
    }

    public void RubyProducts(Toggle toggle)
    {
        UIManager.Instance.ProductParents[0].gameObject.SetActive(toggle.isOn);
    }

    public void CoinProducts(Toggle toggle)
    {
        UIManager.Instance.ProductParents[1].gameObject.SetActive(toggle.isOn);
    }

    public void OvenProducts(Toggle toggle)
    {
        UIManager.Instance.ProductParents[2].gameObject.SetActive(toggle.isOn);
    }

    public void SetActiveDailyUI(bool value)
    {
        UIManager.Instance.DailyUI.Daily.gameObject.SetActive(value);
    }

    public void GetDailyReward()
    {

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