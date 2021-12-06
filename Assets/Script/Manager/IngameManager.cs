using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct ResultWindow
{
    public GameObject go;
    public Image imgResultText;
    public GameObject goReward;
    public Button btnNext;
    public Image imgBtnNextText;
    public RectTransform rtrnStarParent;
    [Header("0: Next, 1: Retry, 2: Clear, 3: Over")] public List<Sprite> spriteTexts;
}

public class IngameManager : MonoBehaviour
{
    public static IngameManager Instance { get; private set; }

    [SerializeField] private Unit ourTower;
    [SerializeField] private Unit theyTower;
    [SerializeField] private List<Tilemap> tmPlatforms;
    [SerializeField] private List<SpriteRenderer> srBgs;
    [SerializeField] private ResultWindow resultWindow;
    [SerializeField] private RectTransform rtrnDamageText;
    [SerializeField] private List<IngameUnitBtnLinker> lkIngameUnitBtns;
    [HideInInspector] public List<Unit> ingameUnits;
    private List<WaveInformation> curWaveDatas;
    private Coroutine CSpawnEnemy;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameStart();
    }

    private void GameStart()
    {
        FieldSetting();
        curWaveDatas = StageManager.Instance.GetWaveData();
        CSpawnEnemy = StartCoroutine(ESpawnEnemy());
        ingameUnits = new List<Unit>();
        MyUnitsSpawnAll();
        for (int i = 0; i < ingameUnits.Count; i++) lkIngameUnitBtns[i].owner = ingameUnits[i];
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
            unit.transform.SetParent(ourTower.transform);
            unit.transform.localScale *= 2;
            ingameUnits.Add(unit);
        }
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
