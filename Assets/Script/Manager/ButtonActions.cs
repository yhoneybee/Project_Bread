using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    /// <param name="theme_number">�׸� ��ȣ (ex : 1)</param>
    public void SetThemeNumber(int theme_number)
    {
        string[] theme_names = { "밝은 오븐", "넓은 등판", "음침한 숲" };

        // �׸� ��ȣ ���� �� �׸� �̸� �ٲ���
        StageInfo.theme_number = theme_number;
        StageInfo.theme_name = theme_names[theme_number - 1];
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
    public void AppearAndHideForPivot(RectTransform RT)
    {
        StartCoroutine(EAppearAndHideForPivot(RT));
    }
    IEnumerator EAppearAndHideForPivot(RectTransform RT)
    {
        if (RT.pivot.x == 0.9f)
        {
            while (RT.pivot.x > 0.005f)
            {
                RT.pivot = Vector2.Lerp(RT.pivot, new Vector2(0, RT.pivot.y), Time.deltaTime * 3);
                yield return new WaitForSeconds(0.001f);
            }
            RT.pivot = new Vector2(0,RT.pivot.y);
        }
        else if (RT.pivot.x == 0)
        {
            while (RT.pivot.x < 0.895f)
            {
                RT.pivot = Vector2.Lerp(RT.pivot, new Vector2(1, RT.pivot.y), Time.deltaTime * 3);
                yield return new WaitForSeconds(0.001f);
            }
            RT.pivot = new Vector2(0.9f,RT.pivot.y);
        }

        yield return null;
    }
}