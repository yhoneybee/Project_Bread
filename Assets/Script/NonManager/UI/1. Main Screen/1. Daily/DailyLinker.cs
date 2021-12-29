using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DailyLinker : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI help_text;
    float hour;
    float minute;
    float second;
    void Start()
    {

    }

    void Update()
    {
        second = 60 - DateTime.Now.Second;
        minute = 60 - DateTime.Now.Minute - 1;
        hour = 24 - DateTime.Now.Hour - 1;


        help_text.text = $"다음 보상까지 {hour} : {minute} : {second}";
    }
}
