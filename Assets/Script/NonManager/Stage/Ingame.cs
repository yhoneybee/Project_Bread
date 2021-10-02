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
            SpawnBread("2�ܰ�������ũ");
        }

        // �ӽ÷� ����� ���� �κ�
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
    /// �Ʊ� ���� �����ϴ� �Լ�
    /// </summary>
    /// <param name="unit_name">Info���� ������ ������ ������ �̸�</param>
    void SpawnBread(string unit_name)
    {
        Unit unit = UnitManager.Instance.GetUnit(unit_name, friendly_tower.position);
        unit.transform.SetParent(friendly_tower);
    }
    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // wave_data�� wave_information���� ��� �ִ� ���� ������ �����̸� �̿��Ͽ� ���̺� ����
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
            // ���� �������� ���� �� �ö��� �� (Lerp�� ���� �� �������� ���� ��Ȯ�� �ö��� ���ϱ� ����)
            if (target_guage - current_guage <= 0.05f)
            {
                // ��Ȯ�� ������ ���� �� target_guage ����
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