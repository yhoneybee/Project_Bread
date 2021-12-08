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
    public Sprite spStar;
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
    [SerializeField] private List<IngameUnitBtnLinker> lkIngameUnitBtns;
    [SerializeField] private StageInfoLinker stageInfoLinker;

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
    private int currentStarCount;
    private int starCount;
    private int fullStarCount;

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
        CheckGameEnd();
        SetFullStarLimitUIs();
    }

    private void GameStart()
    {
        InvokeRepeating(nameof(CountUp), 0, 1);
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
    }

    private void CountUp() => starCount++;

    private void CheckGameEnd()
    {
        if (resultWindow.go.activeSelf) return;
        bool loss = ourTower.Stat.HP <= 0;
        bool win = theyTower.Stat.HP <= 0;

        if (!loss && !win) return;

        StopCoroutine(CSpawnEnemy);
        CancelInvoke(nameof(CountUp));

        resultWindow.go.SetActive(true);
        resultWindow.goReward.SetActive(win);

        resultWindow.imgResultText.sprite = resultWindow.spriteTexts[win ? 2 : 3];
        UIManager.Instance.FixSizeToRatio(resultWindow.imgResultText, 520);

        if (!win) return;

        resultWindow.rtrnStarParent.gameObject.SetActive(true);

        bool firstTry = StageManager.Instance.GetStage().star_count == 0;
        bool fullStarClear = currentStarCount == 3 && StageManager.Instance.GetStage().star_count < 3;

        stageInfoLinker.SetRewards(firstTry, fullStarClear);

        var rewards = stageInfoLinker.GetRewards(firstTry, fullStarClear);
        GameManager.Instance.Coin += rewards.Item1;
        GameManager.Instance.Jem += rewards.Item2;

        if (currentStarCount > StageManager.Instance.GetStage().star_count) StageManager.Instance.GetStage().star_count = currentStarCount;
        StageInfo.stage_number++;
        StageManager.Instance.GetStage().is_startable = true;

        var imgs = resultWindow.rtrnStarParent.GetComponentsInChildren<Image>();
        for (int i = 0; i < currentStarCount; i++) imgs[i].sprite = resultWindow.spStar;

        resultWindow.btnNext.onClick.AddListener(() =>
        {
            ButtonActions.Instance.ChangeScene("E-01_DeckView");
        });
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
        var wait = new WaitForSeconds(0.01f);
        while (Vector2.Distance(tower.transform.position, Camera.main.transform.position) > 0.5f)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(tower.transform.position.x, tower.transform.position.y, -10), Time.deltaTime * 3);
            if (Camera.main.orthographicSize > 3) Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 3, Time.deltaTime * 3);

            yield return wait;
        }

        yield return new WaitForSeconds(1.5f);
    }

    public void DamageText(int damage, Vector2 pos)
    {
        var damageText = Instantiate(rtrnDamageText, pos, Quaternion.identity);
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
            img.gameObject.SetActive(list2[i] != 0);
            if (list2[i] == 0) damageText.GetChild(8).position = img.transform.position;
            img.sprite = GetNumSprite(list2[i]);
        }
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
                        enemy.transform.SetParent(theyTower.transform);
                    }
                    yield return new WaitForSeconds(wave.delay);
                }
                yield return new WaitForSeconds(1);
            }
        }
    }
}
