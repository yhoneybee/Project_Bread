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
    void Start()
    {
        stage_name_text.text = $"{StageInfo.theme_number} - {StageInfo.stage_number}";
        theme_name_text.text = StageInfo.theme_name;

        backgrounds[StageInfo.theme_number - 1].SetActive(true);
        platforms[StageInfo.theme_number - 1].SetActive(true);

        StartCoroutine(SpawnEnemies());
    }
    void Update()
    {

    }
    public void SetTimeScale(float value)
    {
        Time.timeScale = value;
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