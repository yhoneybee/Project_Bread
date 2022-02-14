using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class ButtonActions : MonoBehaviour
{
    public static ButtonActions Instance { get; private set; } = null;

    public static bool directMain;

    private static List<string> lBeforeScene = new List<string>();

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
        string currentScene = SceneManager.GetActiveScene().name;

        if (directMain && name == "C-03_DeckSelect")
        {
            //print("FDSFDSFSD");
        }
        lBeforeScene.Remove(currentScene);
        lBeforeScene.Remove(name);
        lBeforeScene.Add(currentScene);

        directMain = false;
        if (CheckReEntering("B-Main") && name == "D-02_UnitSelect")
        {
            directMain = true;
            print("True");
        }
        StartCoroutine(EChangeScene(name));
    }

    public void BackScene()
    {
        var pop = lBeforeScene[lBeforeScene.Count - 1];
        lBeforeScene.RemoveAt(lBeforeScene.Count - 1);
        if (CheckReEntering("C-03_DeckSelect"))
            directMain = true;
        StartCoroutine(EChangeScene(pop));
    }

    IEnumerator EChangeScene(string name)
    {
        var fade_img = UIManager.Instance.Fade;
        var wait = new WaitForSeconds(0.001f);

        while (fade_img.color.a < 0.95)
        {
            fade_img.color = Color.Lerp(fade_img.color, Color.black, 0.1f);
            yield return wait;
        }
        fade_img.color = Color.black;

        SceneManager.LoadScene(name);

        yield return null;
    }

    public void SetThemeNumber(int theme_number)
    {
        string[] theme_names = { "밝은 오븐", "넓은 등판", "음침한 숲" };

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

    public void UnBoxingOne(int rank)
    {
        int per = Random.Range(1, 101);
        switch (rank)
        {
            case 0: // common
                if (per <= 65)
                {
                    per = Random.Range(1, 101);
                    if (0 < per && per <= 40)
                    {
                        ShopManager.Instance.AddCommon(10);
                    }
                    else if (40 < per && per <= 100)
                    {
                        ShopManager.Instance.AddCommon(7);
                    }
                }
                else
                {
                    per = Random.Range(1, 101);
                    if (0 < per && per <= 20)
                    {
                        ShopManager.Instance.AddRare(7);
                    }
                    else if (20 < per && per <= 50)
                    {
                        ShopManager.Instance.AddRare(5);
                    }
                    else if (50 < per && per <= 100)
                    {
                        ShopManager.Instance.AddRare(3);
                    }
                }
                break;
            case 1: // rare
                if (per <= 65)
                {
                    per = Random.Range(1, 101);
                    if (0 < per && per <= 40)
                    {
                        ShopManager.Instance.AddRare(7);
                    }
                    else if (40 < per && per <= 100)
                    {
                        ShopManager.Instance.AddRare(5);
                    }
                }
                else
                {
                    per = Random.Range(1, 101);
                    if (0 < per && per <= 20)
                    {
                        ShopManager.Instance.AddEpic(5);
                    }
                    else if (20 < per && per <= 50)
                    {
                        ShopManager.Instance.AddEpic(3);
                    }
                    else if (50 < per && per <= 100)
                    {
                        ShopManager.Instance.AddEpic(2);
                    }
                }
                break;
            case 2: // epic
                if (per <= 65)
                {
                    per = Random.Range(1, 101);
                    if (0 < per && per <= 40)
                    {
                        ShopManager.Instance.AddEpic(5);
                    }
                    else if (40 < per && per <= 100)
                    {
                        ShopManager.Instance.AddEpic(3);
                    }
                }
                else
                {
                    per = Random.Range(1, 101);
                    if (0 < per && per <= 20)
                    {
                        ShopManager.Instance.AddLegend(3);
                    }
                    else if (20 < per && per <= 50)
                    {
                        ShopManager.Instance.AddLegend(2);
                    }
                    else if (50 < per && per <= 100)
                    {
                        ShopManager.Instance.AddLegend(1);
                    }
                }
                break;
            case 3: // legend
                break;
        }

        //ShopManager.Instance.Unboxing();
    }

    public void UnBoxingTen(int rank)
    {
        for (int i = 0; i < 11; i++)
        {
            UnBoxingOne(rank);
        }

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