using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectTheme : MonoBehaviour
{
    [SerializeField] Canvas msg_canvas;
    // 화면에 나타나는 안내 문구 프리팹
    [SerializeField] GameObject help_text_prefab;
    GameObject help_text_obj;

    [SerializeField] ParticleSystem[] celebrationParticles = new ParticleSystem[2];

    [SerializeField] Image[] theme_images;
    [SerializeField] Sprite[] theme_sprites;
    [SerializeField] Sprite dont_startable_sprite;

    bool[] is_theme_startable;

    void Start()
    {
        is_theme_startable = new bool[GameManager.Instance.theme_count];

        for (int i = 0; i < is_theme_startable.Length; i++)
        {
            is_theme_startable[i] = StageManager.Instance.GetStage(i, 0).is_startable;
            theme_images[i].sprite = (is_theme_startable[i] ? theme_sprites[i] : dont_startable_sprite);
        }
    }

    void Update()
    {
        if (StageManager.Instance.theme_clear || Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ShowCelebrationMsg());
        }
    }


    Coroutine text_fade = null;
    Coroutine panel_fade = null;
    public void LoadTheme(int theme_number)
    {
        if (help_text_prefab)
        {

            if (theme_number <= GameManager.Instance.theme_count)
            {
                if (theme_number != 1 && !is_theme_startable[theme_number - 1])
                {
                    if (help_text_obj)
                    {
                        StopCoroutine(text_fade);
                        StopCoroutine(panel_fade);
                        Destroy(help_text_obj);
                    }

                    help_text_obj = Instantiate(help_text_prefab, msg_canvas.transform);
                    var text = help_text_obj.GetComponentInChildren<TextMeshProUGUI>();

                    text.text = $"에피소드 {theme_number - 1}을(를) 클리어해주세요.";

                    text_fade = StartCoroutine(FadeOut(text));
                    panel_fade = StartCoroutine(FadeOut(help_text_obj.GetComponent<Image>()));
                }
                else
                {
                    ButtonActions.Instance.SetThemeNumber(theme_number);
                    ButtonActions.Instance.ChangeScene("D-01_StageSelect");
                }
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

    IEnumerator ShowCelebrationMsg()
    {
        GameObject help_text_obj = Instantiate(help_text_prefab, msg_canvas.transform);
        var text = help_text_obj.GetComponentInChildren<TextMeshProUGUI>();
        text.text = $"에피소드 {StageInfo.theme_number - 1}을(를) 클리어하였습니다!\n" +
            $"다음 에피소드로 입장 가능합니다.";

        var help_text_tr = help_text_obj.GetComponent<RectTransform>();
        help_text_tr.sizeDelta = new Vector2(help_text_tr.sizeDelta.x, help_text_tr.sizeDelta.y * 2);

        celebrationParticles[0].Play();
        yield return new WaitForSeconds(0.15f);
        celebrationParticles[1].Play();
        yield return new WaitForSeconds(0.5f);
        celebrationParticles[2].Play();
        yield return new WaitForSeconds(0.5f);
        celebrationParticles[3].Play();

        StageManager.Instance.theme_clear = false;
        StartCoroutine(FadeOut(text));
        StartCoroutine(FadeOut(help_text_obj.GetComponent<Image>()));
    }
}