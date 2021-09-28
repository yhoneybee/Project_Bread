using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{
    public static ButtonActions Instance { get; private set; } = null;

    private void Awake()
    {
        Instance = this;
    }

    public bool CheckReEntering(string name)
    {
        string scene = SceneManager.GetActiveScene().name;
        return name == scene;
    }
    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    /// <summary>
    /// 테마 선택 후 호출해줘야 하는 함수
    /// </summary>
    /// <param name="theme_number">테마 번호</param>
    public void SetThemeNumber(int theme_number)
    {
        string[] theme_names = { "밝은 오븐", "넓은 들판", "음침한 숲" };

        // 테마 번호 저장 후 테마 이름 바꿔줌
        StageInfo.theme_number = theme_number;
        StageInfo.theme_name = theme_names[theme_number];
    }
    public void ChangeDeck(int index)
    {
        GameManager.Instance.Index = index;
    }
    public void ExceptUnit()
    {
        GameManager.Select[GameManager.SelectSlotIdx] = null;
        ChangeScene("C - 03 DeckSelect");
    }
}