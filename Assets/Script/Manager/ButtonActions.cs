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
    /// �׸� ���� �� ȣ������� �ϴ� �Լ�
    /// </summary>
    /// <param name="theme_number">�׸� ��ȣ</param>
    public void SetThemeNumber(int theme_number)
    {
        string[] theme_names = { "���� ����", "���� ����", "��ħ�� ��" };

        // �׸� ��ȣ ���� �� �׸� �̸� �ٲ���
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