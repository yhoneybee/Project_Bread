using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// stage_name : �������� �̸�
/// theme_name : �׸� �̸�
/// stage_dificulty : �������� ���̵� (1 ~ 3)
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
