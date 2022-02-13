using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct StageUiInfo
{
    public Sprite[] spArrThemeName;
    public Image imgThemeNum;
    public Image[] imgArrStageNum;
    public Image imgThemeName;
}
[System.Serializable]
public struct StarLimit
{
    public Slider sliderGague;
    public Image[] imgArrLine;
    public Image[] imgArrStar;
}
[System.Serializable]
public struct ResultWindow
{
    public GameObject go;
    public Image imgResultText;
    public GameObject goReward;
    public Button btnNext;
    public Image imgBtnNextText;
    public RectTransform rtrnStarParent;
    public Image[] yellowStars;
    [Header("0: Next, 1: Retry, 2: Clear, 3: Over")] public List<Sprite> spriteTexts;
}

public class IngameManager : MonoBehaviour
{
    public static IngameManager Instance { get; private set; }

    [SerializeField] private Unit ourTower;
    [SerializeField] private Unit theyTower;
    [SerializeField] private Animator animrTowerDestroy;
    [SerializeField] private List<Tilemap> tmPlatforms;
    [SerializeField] private List<SpriteRenderer> srBgs;
    [SerializeField] private ResultWindow resultWindow;
    [SerializeField] private StarLimit starLimit;
    [SerializeField] private StageUiInfo stageUiInfo;
    [SerializeField] private RectTransform rtrnDamageText;
    [SerializeField] private RectTransform lkIngameUnitBtnsParent;
    [SerializeField] private List<IngameUnitBtnLinker> lkIngameUnitBtns;
    [SerializeField] private StageInfoLinker stageInfoLinker;
    [SerializeField] private RectTransform rtrnDeckParent;

    private List<RectTransform> rtrnDamageTextPool = new List<RectTransform>();
    private Coroutine CTowerDestory;

    public List<Unit> IngameUnits
    {
        get
        {
            if (ingameUnits.Count <= 0)
            {
                ourTower.Stat.HP = 0;
                TowerDestory(ourTower);
            }
            return ingameUnits;
        }
        set => ingameUnits = value;
    }
    private List<WaveInformation> curWaveDatas;
    private Coroutine CSpawnEnemy;
    private List<Unit> ingameUnits;
    public List<Unit> ingameEnemies;
    private int currentStarCount;
    private int starCount;
    private int fullStarCount;
    private bool coroutineEnd;
    private bool isTheyTowerDestroy;
    private bool win;
    private bool loss;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameStart();
    }

    private void Update()
    {
        loss = ourTower.Stat.HP <= 0;
        win = theyTower.Stat.HP <= 0;
        if (win && !isTheyTowerDestroy)
        {
            isTheyTowerDestroy = true;
            TowerDestory(theyTower);
        }
        CheckGameEnd();
        SetFullStarLimitUIs();
    }

    private void GameStart()
    {
        InvokeRepeating(nameof(CountUp), 0, 1);
        stageUiInfo.imgThemeNum.sprite = GetNumSprite(StageInfo.theme_number);
        stageUiInfo.imgThemeName.sprite = stageUiInfo.spArrThemeName[StageInfo.theme_number - 1];
        stageUiInfo.imgArrStageNum[0].gameObject.SetActive(StageInfo.stage_number >= 10);
        stageUiInfo.imgArrStageNum[0].sprite = GetNumSprite(StageInfo.stage_number / 10);
        stageUiInfo.imgArrStageNum[1].sprite = GetNumSprite(StageInfo.stage_number % 10);
        starCount = 0;
        currentStarCount = 3;
        fullStarCount = 120;
        FieldSetting();
        curWaveDatas = StageManager.Instance.GetWaveData();
        CSpawnEnemy = StartCoroutine(ESpawnEnemy());
        ingameUnits = new List<Unit>();
        MyUnitsSpawnAll();
        for (int i = 0; i < ingameUnits.Count; i++)
            lkIngameUnitBtns[i].owner = ingameUnits[i];
        lkIngameUnitBtnsParent.gameObject.SetActive(true);
    }

    private void CountUp() => starCount++;

    private void CheckGameEnd()
    {
        if (resultWindow.go.activeSelf || !coroutineEnd) return;

        if (!loss && !win) return;

        StopCoroutine(CSpawnEnemy);
        CancelInvoke(nameof(CountUp));

        resultWindow.go.SetActive(true);
        resultWindow.goReward.SetActive(win);

        resultWindow.imgResultText.sprite = resultWindow.spriteTexts[win ? 2 : 3];
        UIManager.Instance.FixSizeToRatio(resultWindow.imgResultText, 520);

        SoundManager.Instance.Play(win ? "SFX/Stage Result/Stage Clear" : "SFX/Stage Result/Stage Fail", SoundType.EFFECT);

        if (!win) return;

        resultWindow.rtrnStarParent.gameObject.SetActive(true);

        bool firstTry = StageManager.Instance.GetStage().star_count == 0;
        bool fullStarClear = currentStarCount == 3 && StageManager.Instance.GetStage().star_count < 3;

        stageInfoLinker.SetRewards(firstTry, fullStarClear);

        var rewards = stageInfoLinker.GetRewards(firstTry, fullStarClear);
        GameManager.Instance.Coin += rewards.Item1;
        GameManager.Instance.Jem += rewards.Item2;

        if (currentStarCount > StageManager.Instance.GetStage().star_count) StageManager.Instance.GetStage().star_count = currentStarCount;

        StartCoroutine(ClearAnimation(currentStarCount));

        if (StageInfo.stage_number == 10)
        {
            var mainBtnObj = GameObject.Find("Go To Main");
            Destroy(mainBtnObj);

            resultWindow.btnNext.transform.localPosition =
                new Vector3(0, resultWindow.btnNext.transform.localPosition.y);

            resultWindow.btnNext.onClick.AddListener(() =>
            {
                GameManager.Instance.GameCount++;
                ButtonActions.Instance.ChangeScene("C-01_ThemeSelect");

                if (!StageManager.Instance.GetStage(StageInfo.theme_number, 0).is_startable)
                {
                    StageManager.Instance.theme_clear = true;
                }
            });
        }
        else
        {
            resultWindow.btnNext.onClick.AddListener(() =>
            {
                ButtonActions.Instance.ChangeScene("E-01_DeckView");
            });
        }

        StageInfo.stage_number++;
        StageManager.Instance.GetStage().is_startable = true;

        if (firstTry)
        {
            switch (StageInfo.theme_number)
            {
                case 1:
                    switch (StageInfo.stage_number)
                    {
                        case 1:
                            GameManager.Instance.Coin += 2500;
                            GameManager.Instance.Jem += 50;
                            GameManager.Instance.PlayerExp += 250;
                            break;
                        case 2:
                            GameManager.Instance.Coin += 2500;
                            GameManager.Instance.Jem += 50;
                            GameManager.Instance.PlayerExp += 250;
                            break;
                        case 3:
                            GameManager.Instance.Coin += 3000;
                            GameManager.Instance.Jem += 100;
                            GameManager.Instance.PlayerExp += 250;
                            break;
                        case 4:
                            GameManager.Instance.Coin += 3000;
                            GameManager.Instance.Jem += 100;
                            GameManager.Instance.PlayerExp += 250;
                            break;
                        case 5:
                            GameManager.Instance.Coin += 3500;
                            GameManager.Instance.Jem += 150;
                            GameManager.Instance.PlayerExp += 250;
                            break;
                        case 6:
                            GameManager.Instance.Coin += 3500;
                            GameManager.Instance.Jem += 150;
                            GameManager.Instance.PlayerExp += 250;
                            break;
                        case 7:
                            GameManager.Instance.Coin += 4000;
                            GameManager.Instance.Jem += 200;
                            GameManager.Instance.PlayerExp += 250;
                            break;
                        case 8:
                            GameManager.Instance.Coin += 4000;
                            GameManager.Instance.Jem += 200;
                            GameManager.Instance.PlayerExp += 250;
                            break;
                        case 9:
                            GameManager.Instance.Coin += 4500;
                            GameManager.Instance.Jem += 250;
                            GameManager.Instance.PlayerExp += 250;
                            break;
                        case 10:
                            GameManager.Instance.Coin += 4500;
                            GameManager.Instance.Jem += 250;
                            GameManager.Instance.PlayerExp += 250;
                            break;
                    }
                    break;
                case 2:
                    switch (StageInfo.stage_number)
                    {
                        case 1:
                            GameManager.Instance.Coin += 5000;
                            GameManager.Instance.Jem += 300;
                            GameManager.Instance.PlayerExp += 300;
                            break;
                        case 2:
                            GameManager.Instance.Coin += 5000;
                            GameManager.Instance.Jem += 300;
                            GameManager.Instance.PlayerExp += 300;
                            break;
                        case 3:
                            GameManager.Instance.Coin += 6000;
                            GameManager.Instance.Jem += 400;
                            GameManager.Instance.PlayerExp += 300;
                            break;
                        case 4:
                            GameManager.Instance.Coin += 6000;
                            GameManager.Instance.Jem += 400;
                            GameManager.Instance.PlayerExp += 300;
                            break;
                        case 5:
                            GameManager.Instance.Coin += 7000;
                            GameManager.Instance.Jem += 500;
                            GameManager.Instance.PlayerExp += 300;
                            break;
                        case 6:
                            GameManager.Instance.Coin += 7000;
                            GameManager.Instance.Jem += 500;
                            GameManager.Instance.PlayerExp += 300;
                            break;
                        case 7:
                            GameManager.Instance.Coin += 8000;
                            GameManager.Instance.Jem += 600;
                            GameManager.Instance.PlayerExp += 300;
                            break;
                        case 8:
                            GameManager.Instance.Coin += 8000;
                            GameManager.Instance.Jem += 600;
                            GameManager.Instance.PlayerExp += 300;
                            break;
                        case 9:
                            GameManager.Instance.Coin += 9000;
                            GameManager.Instance.Jem += 700;
                            GameManager.Instance.PlayerExp += 300;
                            break;
                        case 10:
                            GameManager.Instance.Coin += 9000;
                            GameManager.Instance.Jem += 700;
                            GameManager.Instance.PlayerExp += 300;
                            break;
                    }
                    break;
                case 3:
                    switch (StageInfo.stage_number)
                    {
                        case 1:
                            GameManager.Instance.Coin += 10000;
                            GameManager.Instance.Jem += 800;
                            GameManager.Instance.PlayerExp += 350;
                            break;
                        case 2:
                            GameManager.Instance.Coin += 10000;
                            GameManager.Instance.Jem += 800;
                            GameManager.Instance.PlayerExp += 350;
                            break;
                        case 3:
                            GameManager.Instance.Coin += 12000;
                            GameManager.Instance.Jem += 1000;
                            GameManager.Instance.PlayerExp += 350;
                            break;
                        case 4:
                            GameManager.Instance.Coin += 12000;
                            GameManager.Instance.Jem += 1000;
                            GameManager.Instance.PlayerExp += 350;
                            break;
                        case 5:
                            GameManager.Instance.Coin += 14000;
                            GameManager.Instance.Jem += 1200;
                            GameManager.Instance.PlayerExp += 350;
                            break;
                        case 6:
                            GameManager.Instance.Coin += 14000;
                            GameManager.Instance.Jem += 1200;
                            GameManager.Instance.PlayerExp += 350;
                            break;
                        case 7:
                            GameManager.Instance.Coin += 16000;
                            GameManager.Instance.Jem += 1400;
                            GameManager.Instance.PlayerExp += 350;
                            break;
                        case 8:
                            GameManager.Instance.Coin += 16000;
                            GameManager.Instance.Jem += 1400;
                            GameManager.Instance.PlayerExp += 350;
                            break;
                        case 9:
                            GameManager.Instance.Coin += 18000;
                            GameManager.Instance.Jem += 1600;
                            GameManager.Instance.PlayerExp += 350;
                            break;
                        case 10:
                            GameManager.Instance.Coin += 18000;
                            GameManager.Instance.Jem += 1600;
                            GameManager.Instance.PlayerExp += 350;
                            break;
                    }
                    break;
            }
        }
        if (fullStarClear)
        {
            switch (StageInfo.theme_number)
            {
                case 1:
                    switch (StageInfo.stage_number)
                    {
                        case 1:
                            GameManager.Instance.Coin += 3000;
                            GameManager.Instance.Jem += 10;
                            GameManager.Instance.PlayerExp += 300;
                            break;
                        case 2:
                            GameManager.Instance.Coin += 3000;
                            GameManager.Instance.Jem += 10;
                            GameManager.Instance.PlayerExp += 300;
                            break;
                        case 3:
                            GameManager.Instance.Coin += 3500;
                            GameManager.Instance.Jem += 150;
                            GameManager.Instance.PlayerExp += 300;
                            break;
                        case 4:
                            GameManager.Instance.Coin += 3500;
                            GameManager.Instance.Jem += 150;
                            GameManager.Instance.PlayerExp += 300;
                            break;
                        case 5:
                            GameManager.Instance.Coin += 4000;
                            GameManager.Instance.Jem += 200;
                            GameManager.Instance.PlayerExp += 300;
                            break;
                        case 6:
                            GameManager.Instance.Coin += 4000;
                            GameManager.Instance.Jem += 200;
                            GameManager.Instance.PlayerExp += 300;
                            break;
                        case 7:
                            GameManager.Instance.Coin += 4500;
                            GameManager.Instance.Jem += 250;
                            GameManager.Instance.PlayerExp += 300;
                            break;
                        case 8:
                            GameManager.Instance.Coin += 4500;
                            GameManager.Instance.Jem += 250;
                            GameManager.Instance.PlayerExp += 300;
                            break;
                        case 9:
                            GameManager.Instance.Coin += 5000;
                            GameManager.Instance.Jem += 300;
                            GameManager.Instance.PlayerExp += 300;
                            break;
                        case 10:
                            GameManager.Instance.Coin += 5000;
                            GameManager.Instance.Jem += 300;
                            GameManager.Instance.PlayerExp += 300;
                            break;
                    }
                    break;
                case 2:
                    switch (StageInfo.stage_number)
                    {
                        case 1:
                            GameManager.Instance.Coin += 5500;
                            GameManager.Instance.Jem += 350;
                            GameManager.Instance.PlayerExp += 350;
                            break;
                        case 2:
                            GameManager.Instance.Coin += 5500;
                            GameManager.Instance.Jem += 350;
                            GameManager.Instance.PlayerExp += 350;
                            break;
                        case 3:
                            GameManager.Instance.Coin += 6500;
                            GameManager.Instance.Jem += 450;
                            GameManager.Instance.PlayerExp += 350;
                            break;
                        case 4:
                            GameManager.Instance.Coin += 6500;
                            GameManager.Instance.Jem += 450;
                            GameManager.Instance.PlayerExp += 350;
                            break;
                        case 5:
                            GameManager.Instance.Coin += 7500;
                            GameManager.Instance.Jem += 550;
                            GameManager.Instance.PlayerExp += 350;
                            break;
                        case 6:
                            GameManager.Instance.Coin += 7500;
                            GameManager.Instance.Jem += 550;
                            GameManager.Instance.PlayerExp += 350;
                            break;
                        case 7:
                            GameManager.Instance.Coin += 8500;
                            GameManager.Instance.Jem += 650;
                            GameManager.Instance.PlayerExp += 350;
                            break;
                        case 8:
                            GameManager.Instance.Coin += 8500;
                            GameManager.Instance.Jem += 650;
                            GameManager.Instance.PlayerExp += 350;
                            break;
                        case 9:
                            GameManager.Instance.Coin += 9500;
                            GameManager.Instance.Jem += 750;
                            GameManager.Instance.PlayerExp += 350;
                            break;
                        case 10:
                            GameManager.Instance.Coin += 9500;
                            GameManager.Instance.Jem += 750;
                            GameManager.Instance.PlayerExp += 350;
                            break;
                    }
                    break;
                case 3:
                    switch (StageInfo.stage_number)
                    {
                        case 1:
                            GameManager.Instance.Coin += 10500;
                            GameManager.Instance.Jem += 850;
                            GameManager.Instance.PlayerExp += 400;
                            break;
                        case 2:
                            GameManager.Instance.Coin += 10500;
                            GameManager.Instance.Jem += 850;
                            GameManager.Instance.PlayerExp += 400;
                            break;
                        case 3:
                            GameManager.Instance.Coin += 12500;
                            GameManager.Instance.Jem += 1050;
                            GameManager.Instance.PlayerExp += 400;
                            break;
                        case 4:
                            GameManager.Instance.Coin += 12500;
                            GameManager.Instance.Jem += 1050;
                            GameManager.Instance.PlayerExp += 400;
                            break;
                        case 5:
                            GameManager.Instance.Coin += 14500;
                            GameManager.Instance.Jem += 1250;
                            GameManager.Instance.PlayerExp += 400;
                            break;
                        case 6:
                            GameManager.Instance.Coin += 14500;
                            GameManager.Instance.Jem += 1250;
                            GameManager.Instance.PlayerExp += 400;
                            break;
                        case 7:
                            GameManager.Instance.Coin += 16500;
                            GameManager.Instance.Jem += 1450;
                            GameManager.Instance.PlayerExp += 400;
                            break;
                        case 8:
                            GameManager.Instance.Coin += 16500;
                            GameManager.Instance.Jem += 1450;
                            GameManager.Instance.PlayerExp += 400;
                            break;
                        case 9:
                            GameManager.Instance.Coin += 18500;
                            GameManager.Instance.Jem += 1650;
                            GameManager.Instance.PlayerExp += 400;
                            break;
                        case 10:
                            GameManager.Instance.Coin += 18500;
                            GameManager.Instance.Jem += 1650;
                            GameManager.Instance.PlayerExp += 400;
                            break;
                    }
                    break;
            }
        }

        switch (StageInfo.theme_number)
        {
            case 1:
                switch (StageInfo.stage_number)
                {
                    case 1:
                        GameManager.Instance.Coin += 500;
                        GameManager.Instance.Jem += 5;
                        GameManager.Instance.PlayerExp += 50;
                        break;
                    case 2:
                        GameManager.Instance.Coin += 500;
                        GameManager.Instance.Jem += 5;
                        GameManager.Instance.PlayerExp += 50;
                        break;
                    case 3:
                        GameManager.Instance.Coin += 550;
                        GameManager.Instance.Jem += 5;
                        GameManager.Instance.PlayerExp += 50;
                        break;
                    case 4:
                        GameManager.Instance.Coin += 550;
                        GameManager.Instance.Jem += 5;
                        GameManager.Instance.PlayerExp += 50;
                        break;
                    case 5:
                        GameManager.Instance.Coin += 600;
                        GameManager.Instance.Jem += 5;
                        GameManager.Instance.PlayerExp += 50;
                        break;
                    case 6:
                        GameManager.Instance.Coin += 600;
                        GameManager.Instance.Jem += 10;
                        GameManager.Instance.PlayerExp += 50;
                        break;
                    case 7:
                        GameManager.Instance.Coin += 650;
                        GameManager.Instance.Jem += 10;
                        GameManager.Instance.PlayerExp += 50;
                        break;
                    case 8:
                        GameManager.Instance.Coin += 650;
                        GameManager.Instance.Jem += 10;
                        GameManager.Instance.PlayerExp += 50;
                        break;
                    case 9:
                        GameManager.Instance.Coin += 700;
                        GameManager.Instance.Jem += 10;
                        GameManager.Instance.PlayerExp += 50;
                        break;
                    case 10:
                        GameManager.Instance.Coin += 700;
                        GameManager.Instance.Jem += 10;
                        GameManager.Instance.PlayerExp += 50;
                        break;
                }
                break;
            case 2:
                switch (StageInfo.stage_number)
                {
                    case 1:
                        GameManager.Instance.Coin += 1000;
                        GameManager.Instance.Jem += 20;
                        GameManager.Instance.PlayerExp += 75;
                        break;
                    case 2:
                        GameManager.Instance.Coin += 1000;
                        GameManager.Instance.Jem += 20;
                        GameManager.Instance.PlayerExp += 75;
                        break;
                    case 3:
                        GameManager.Instance.Coin += 1100;
                        GameManager.Instance.Jem += 20;
                        GameManager.Instance.PlayerExp += 75;
                        break;
                    case 4:
                        GameManager.Instance.Coin += 1100;
                        GameManager.Instance.Jem += 20;
                        GameManager.Instance.PlayerExp += 75;
                        break;
                    case 5:
                        GameManager.Instance.Coin += 1200;
                        GameManager.Instance.Jem += 20;
                        GameManager.Instance.PlayerExp += 75;
                        break;
                    case 6:
                        GameManager.Instance.Coin += 1200;
                        GameManager.Instance.Jem += 30;
                        GameManager.Instance.PlayerExp += 75;
                        break;
                    case 7:
                        GameManager.Instance.Coin += 1300;
                        GameManager.Instance.Jem += 30;
                        GameManager.Instance.PlayerExp += 75;
                        break;
                    case 8:
                        GameManager.Instance.Coin += 1300;
                        GameManager.Instance.Jem += 30;
                        GameManager.Instance.PlayerExp += 75;
                        break;
                    case 9:
                        GameManager.Instance.Coin += 1400;
                        GameManager.Instance.Jem += 30;
                        GameManager.Instance.PlayerExp += 75;
                        break;
                    case 10:
                        GameManager.Instance.Coin += 1400;
                        GameManager.Instance.Jem += 30;
                        GameManager.Instance.PlayerExp += 75;
                        break;
                }
                break;
            case 3:
                switch (StageInfo.stage_number)
                {
                    case 1:
                        GameManager.Instance.Coin += 2000;
                        GameManager.Instance.Jem += 50;
                        GameManager.Instance.PlayerExp += 100;
                        break;
                    case 2:
                        GameManager.Instance.Coin += 2000;
                        GameManager.Instance.Jem += 50;
                        GameManager.Instance.PlayerExp += 100;
                        break;
                    case 3:
                        GameManager.Instance.Coin += 2200;
                        GameManager.Instance.Jem += 50;
                        GameManager.Instance.PlayerExp += 100;
                        break;
                    case 4:
                        GameManager.Instance.Coin += 2200;
                        GameManager.Instance.Jem += 50;
                        GameManager.Instance.PlayerExp += 100;
                        break;
                    case 5:
                        GameManager.Instance.Coin += 2400;
                        GameManager.Instance.Jem += 50;
                        GameManager.Instance.PlayerExp += 100;
                        break;
                    case 6:
                        GameManager.Instance.Coin += 2400;
                        GameManager.Instance.Jem += 100;
                        GameManager.Instance.PlayerExp += 100;
                        break;
                    case 7:
                        GameManager.Instance.Coin += 2600;
                        GameManager.Instance.Jem += 100;
                        GameManager.Instance.PlayerExp += 100;
                        break;
                    case 8:
                        GameManager.Instance.Coin += 2600;
                        GameManager.Instance.Jem += 100;
                        GameManager.Instance.PlayerExp += 100;
                        break;
                    case 9:
                        GameManager.Instance.Coin += 2800;
                        GameManager.Instance.Jem += 100;
                        GameManager.Instance.PlayerExp += 100;
                        break;
                    case 10:
                        GameManager.Instance.Coin += 2800;
                        GameManager.Instance.Jem += 100;
                        GameManager.Instance.PlayerExp += 100;
                        break;
                }
                break;
        }
    }

    void SetFullStarLimitUIs()
    {
        float value = 1 - starCount / (float)(fullStarCount);
        starLimit.sliderGague.value = value;

        int index;

        switch (starLimit.sliderGague.value)
        {
            case float f when (f == 0f):
                index = 0;
                currentStarCount = 1;
                foreach (var image in starLimit.sliderGague.GetComponentsInChildren<Image>())
                    if (image.enabled)
                        FadeOutImage(image);
                break;
            case float f when (f < 0.33f):
                index = 1;
                currentStarCount = 2;
                break;
            default:
                return;
        }

        // 이미지 fade out해야될 경우 해당 이미지를 fade out 처리
        if (starLimit.imgArrStar[index].enabled)
            FadeOutImage(starLimit.imgArrStar[index]);

        if (starLimit.imgArrLine[index].enabled)
            FadeOutImage(starLimit.imgArrLine[index]);
    }

    private void FadeOutImage(Image img)
    {
        img.color = Color.Lerp(img.color, new Color(img.color.r, img.color.g, img.color.b, 0), 0.1f);
        img.enabled = img.color.a > 0.01f;
    }

    private void FieldSetting()
    {
        int curStageIdx = StageInfo.theme_number - 1;
        tmPlatforms[curStageIdx].gameObject.SetActive(true);
        srBgs[curStageIdx].gameObject.SetActive(true);
    }

    private void MyUnitsSpawnAll()
    {
        for (int i = 0; i < DeckManager.Select.Count; i++)
        {
            SpawnUnit(i);
        }
    }

    private void SpawnUnit(int i)
    {
        if (DeckManager.Select[i])
        {
            var unit = UnitManager.Instance.GetUnit(DeckManager.Select[i].Info.Name, ourTower.transform.position);
            unit.GetComponent<SpriteRenderer>().sortingOrder = i;
            unit.transform.SetParent(ourTower.transform);
            unit.transform.localScale *= 2;
            ingameUnits.Add(unit);
        }
    }

    private void TowerDestory(Unit tower)
    {
        print("<color=red>BOOM</color>");
        StartCoroutine(ETowerDestory(tower));
        tower.GetComponent<Animator>().SetTrigger("Destroy");
    }

    IEnumerator ETowerDestory(Unit tower)
    {
        coroutineEnd = false;
        var wait = new WaitForSeconds(0.01f);
        while (Vector2.Distance(tower.transform.position, Camera.main.transform.position) > 0.5f)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(tower.transform.position.x, tower.transform.position.y, -10), Time.deltaTime * 3);
            if (Camera.main.orthographicSize > 3) Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 3, Time.deltaTime * 3);
            rtrnDeckParent.position = Vector3.MoveTowards(rtrnDeckParent.position, rtrnDeckParent.position + Vector3.down * 5, 3000);
            yield return wait;
        }

        yield return new WaitForSeconds(1.5f);
        coroutineEnd = true;
        yield return null;
    }

    public void DamageText(int damage, Vector2 pos)
    {
        StartCoroutine(_DamageText(damage, pos));
    }

    RectTransform GetDamageText()
    {
        RectTransform rtrn;

        if (rtrnDamageTextPool.Count > 0)
        {
            rtrn = rtrnDamageTextPool[0];
            rtrnDamageTextPool.RemoveAt(0);
            rtrn.gameObject.SetActive(true);
        }
        else
        {
            rtrn = Instantiate(rtrnDamageText);
        }

        return rtrn;
    }

    public void ReturnDamageText(RectTransform rtrn)
    {
        rtrn.gameObject.SetActive(false);
        rtrnDamageTextPool.Add(rtrn);
    }

    IEnumerator _DamageText(int damage, Vector2 pos)
    {
        yield return null;

        Vector2 spawn_position = pos + Vector2.up;
        var damageText = GetDamageText();
        damageText.transform.position = spawn_position;
        List<int> list2 = new List<int>()
        {
            (damage % 10000000) / 1000000,
            (damage % 1000000) / 100000,
            (damage % 100000) / 10000,
            (damage % 10000) / 1000,
            (damage % 1000) / 100,
            (damage % 100) / 10,
            (damage % 10),
        };
        for (int i = 0; i < 7; i++)
        {
            var img = damageText.GetChild(i).GetComponent<SpriteRenderer>();
            float pow = Mathf.Pow(10, 7 - i);
            img.gameObject.SetActive(list2[i] != 0 || pow <= damage);
            if (list2[i] == 0 && pow > damage) damageText.GetChild(8).position = img.transform.position;
            img.sprite = GetNumSprite(list2[i]);
        }

        yield return new WaitForSeconds(0.5f);
    }

    private Sprite GetNumSprite(int num) => UIManager.Instance.Nums[num == 0 ? 9 : num - 1];

    IEnumerator ESpawnEnemy()
    {
        while (true)
        {
            for (int i = 0; i < curWaveDatas.Count; i++)
            {
                foreach (var wave in curWaveDatas[i].wave_information)
                {
                    if (wave.unit)
                    {
                        var enemy = UnitManager.Instance.GetUnit(wave.unit.Info.Name, theyTower.transform.position);
                        ingameEnemies.Add(enemy);
                        enemy.transform.SetParent(theyTower.transform);
                    }
                    yield return new WaitForSeconds(wave.delay);
                }
                yield return new WaitForSeconds(1);
            }
        }
    }

    IEnumerator ClearAnimation(int star_count)
    {
        Debug.Log(star_count);
        WaitForSeconds second = new WaitForSeconds(0.01f);
        Image image;

        for (int i = 0; i < star_count; i++)
        {
            image = resultWindow.yellowStars[i];

            while (image.color.a < 0.9f)
            {
                image.color = Color.Lerp(image.color, Color.white, 0.2f);
                yield return second;
            }
            image.color = Color.white;

            while (image.rectTransform.localScale.x > 1.01f)
            {
                image.rectTransform.localScale = Vector2.MoveTowards(image.rectTransform.localScale, Vector2.one, 0.3f);
                yield return second;
            }
            image.rectTransform.localScale = Vector2.one;

            image.GetComponentInChildren<ParticleSystem>().Play();

            SoundManager.Instance.Play("SFX/Stage Result/Star Sound", SoundType.EFFECT);

            yield return new WaitForSeconds(0.2f);
        }
    }
}
