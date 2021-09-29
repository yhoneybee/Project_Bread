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
    void Start()
    {
        stage_name_text.text = StageInfo.stage_name;
        theme_name_text.text = StageInfo.theme_name;

        backgrounds[StageInfo.theme_number - 1].SetActive(true);
        platforms[StageInfo.theme_number - 1].SetActive(true);
    }
    void Update()
    {
        
    }
}
