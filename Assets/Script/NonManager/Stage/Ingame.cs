using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ingame : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI stage_name_text;
    [SerializeField] TextMeshProUGUI theme_name_text;

    [SerializeField] GameObject[] backgrounds;
    [SerializeField] GameObject[] platforms;

    [SerializeField] Transform friendly_tower;
    [SerializeField] Transform enemy_tower;

    [SerializeField] WaveData wave_data;

    [SerializeField] Unit[] units;
    [SerializeField] Transform unit_pool;
    void Start()
    {
        stage_name_text.text = $"{StageInfo.theme_number} - {StageInfo.stage_number}";
        theme_name_text.text = StageInfo.theme_name;

        backgrounds[StageInfo.theme_number - 1].SetActive(true);
        platforms[StageInfo.theme_number - 1].SetActive(true);

        foreach (var unit in units)
        {
            UnitManager.Instance.ReturnUnit(unit, unit_pool);
        }

        StartCoroutine(SpawnEnemies());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnBread("2단과일케이크");
        }
    }
    public void SetTimeScale(float value)
    {
        Time.timeScale = value;
    }

    /// <summary>
    /// 아군 빵을 생성하는 함수
    /// </summary>
    /// <param name="unit_name">Info에서 저장한 생성할 유닛의 이름</param>
    void SpawnBread(string unit_name)
    {
        Unit unit = UnitManager.Instance.GetUnit(unit_name, friendly_tower.position);
        unit.transform.SetParent(friendly_tower);
    }
    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // wave_data의 wave_information에서 담겨 있는 유닛 정보와 딜레이를 이용하여 웨이브 구성
            foreach (var data in wave_data.wave_information)
            {
                if (data.unit != null)
                {
                    Instantiate(data.unit, enemy_tower);
                }
                yield return new WaitForSeconds(data.delay);
            }
        }
    }
}