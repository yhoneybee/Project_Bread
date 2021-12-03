using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 대호 개발, 업적 씬을 총괄하는 클래스
public class AchievementLinker : MonoBehaviour
{
    [SerializeField]
    enum AchiveCase
    {
        Adventure,
        Content,
        System,
        Character,
    }

    [System.Serializable]
    [SerializeField]
    class BasicButton
    {
        public Button button;
        public Image button_image;
        public UnityEngine.Sprite basic_sprite;
        public UnityEngine.Sprite select_sprite;
        public int button_index;
    }
    [SerializeField] BasicButton[] side_buttons;

    [SerializeField] BasicButton[] showing_buttons = new BasicButton[2];

    [SerializeField] Achieve[] achieve;

    BasicButton selected_side_button;
    Coroutine set_color = null;
    Coroutine set_default_color = null;

    private void Start()
    {
        // 첫 번째(Adventure Button) 외의 버튼 비활성화
        selected_side_button = side_buttons[0];
        for (int i = 1; i < side_buttons.Length; i++)
        {
            side_buttons[i].button_image.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }
    private void Update()
    {
    }

    public void PressSideButton(int button_index)
    {
        if (selected_side_button.button_index != button_index)
        {
            if (set_default_color != null) StopCoroutine(set_default_color);
            set_default_color = StartCoroutine(FadeImage(selected_side_button.button_image, new Color(0.5f, 0.5f, 0.5f), true));

            for (int i = 0; i < side_buttons.Length; i++)
            {
                if (side_buttons[i].button_index == button_index)
                    selected_side_button = side_buttons[i];
            }

            if (set_color != null) StopCoroutine(set_color);
            StartCoroutine(FadeImage(selected_side_button.button_image, Color.white));
        }
    }
    public void PressShowingButton(int button_index)
    {
        showing_buttons[button_index].button_image.sprite = showing_buttons[button_index].select_sprite;

        int other_index = Mathf.Abs(button_index - 1);

        showing_buttons[other_index].button_image.sprite = showing_buttons[other_index].basic_sprite;
    }

    IEnumerator FadeImage(Image image, Color color, bool default_color = false)
    {
        yield return null;

        image.color = color;

        // 아 진짜 겁나빡친다 왜 안돼
        /*        float time = Time.time;

                while (Time.time - time < 1)
                {
                    image.color = Color.Lerp(image.color, color, 0.3f);
                    yield return new WaitForSeconds(0.01f);
                }

                if (default_color)
                    set_default_color = null;
                else
                    set_color = null;*/
    }
}