using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField] Slider guage_slider;
    [SerializeField] Text guage_text;

    float current_guage = 0;
    float target_guage = 1;
    void Start()
    {
        stage_name_text.text = $"{StageInfo.theme_number} - {StageInfo.stage_number}";
        theme_name_text.text = StageInfo.theme_name;

        backgrounds[StageInfo.theme_number - 1].SetActive(true);
        platforms[StageInfo.theme_number - 1].SetActive(true);

        StartCoroutine(SpawnEnemies());
        StartCoroutine(Guage_Change());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnBread("2단과일케이크");
        }

        // 임시로 만들어 놓은 부분
        if (Input.GetKeyDown(KeyCode.Return))
        {
            current_guage--;
            target_guage--;
        }

        guage_slider.value = current_guage / 10;
        guage_text.text = ((int)current_guage).ToString();
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
    IEnumerator Guage_Change()
    {
        while (true)
        {
            // 현재 게이지가 거의 다 올라갔을 때 (Lerp를 통한 값 설정으로 인해 정확히 올라가지 못하기 때문)
            if (target_guage - current_guage <= 0.05f)
            {
                // 정확한 값으로 설정 및 target_guage 설정
                current_guage = Mathf.Round(current_guage);
                if (current_guage < 10)
                    target_guage = current_guage + 1;
                yield return new WaitForSeconds(1f);
            }
            current_guage = Mathf.Lerp(current_guage, target_guage, 0.2f);

            yield return new WaitForSeconds(0.01f);
        }
    }
}