using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectTheme : MonoBehaviour
{
    [SerializeField] Canvas main_canvas;
    // 화면에 나타나는 안내 문구 프리팹
    [SerializeField] RectTransform help_text_prefab;

    void Start()
    {

    }

    void Update()
    {

    }
    public void ShowHelpText(int theme_number)
    {
        if (help_text_prefab)
        {
            ButtonActions.Instance.SetThemeNumber(theme_number);

            if (theme_number < 5)
                if (theme_number != 1 && StageManager.Instance.GetStage(StageInfo.theme_number - 2, 9).star_count == 0) // 인덱스로서 접근하기 때문에 빼줌
                {
                    RectTransform help_text_obj = Instantiate(help_text_prefab, main_canvas.transform);
                    var text = help_text_obj.GetComponentInChildren<TextMeshProUGUI>();

                    text.text = $"에피소드 {--StageInfo.theme_number}을(를) 클리어해주세요.";

                    StartCoroutine(FadeOut(text));
                    StartCoroutine(FadeOut(help_text_obj.GetComponent<Image>()));
                }
                else
                {
                    ButtonActions.Instance.ChangeScene("D - 01 StageSelect");
                }
        }
    }

    IEnumerator FadeOut(Image image)
    {
        yield return new WaitForSeconds(2f);
        while (image.color.a > 0.01f)
        {
            yield return new WaitForSeconds(0.01f);
            image.color = Color.Lerp(image.color, new Color(image.color.r, image.color.g, image.color.b, 0), Time.deltaTime * 3);
        }

        if (image)
            Destroy(image.gameObject);
    }
    IEnumerator FadeOut(TextMeshProUGUI text)
    {
        yield return new WaitForSeconds(2f);
        while (text.color.a > 0.01f)
        {
            yield return new WaitForSeconds(0.01f);
            text.color = Color.Lerp(text.color, new Color(text.color.r, text.color.g, text.color.b, 0), Time.deltaTime * 3);
        }

        if (text)
            Destroy(text.gameObject);
    }
}
