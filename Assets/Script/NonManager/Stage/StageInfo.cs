using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// stage_name : 스테이지 이름
/// theme_name : 테마 이름
/// stage_dificulty : 스테이지 난이도 (1 ~ 3)
/// </summary>
public class StageInfo
{
    public static string stage_name { get; set; }
    public static string theme_name { get; set; }
    private static int _stage_number;
    public static int stage_number
    {
        get => _stage_number;
        set
        {
            _stage_number = value;
            while (_stage_number > 10)
            {
                theme_number++;
                _stage_number -= 10;
            }
            stage_name = $"{theme_number}-{stage_number}";
        }
    }
    private static int _theme_number;
    public static int theme_number
    {
        get => _theme_number;
        set
        {
            _theme_number = value;
            stage_name = $"{theme_number}-{stage_number}";
        }
    }

    public static int stage_dificulty { get; set; }
}
