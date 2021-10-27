using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchivementLinker : MonoBehaviour
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
        public Sprite basic_sprite;
        public Sprite select_sprite;
        public int button_index;
    }
    [SerializeField] BasicButton[] side_buttons;

    [SerializeField] BasicButton[] showing_buttons = new BasicButton[2];

    [System.Serializable]
    [SerializeField]
    class AchiveContent
    {
        public AchiveCase achive_case;
        public int achive_index;

        public GameObject achive_content;
        public Button button;
        public Image button_image;
        public Sprite base_sprite;
        public Sprite able_sprite;
        public Sprite taked_sprite;
    }
    [SerializeField] AchiveContent[] achive_contents;

    BasicButton select_side_button;
    Coroutine set_color = null;
    Coroutine set_default_color = null;

    private void Start()
    {
        // ù ��°(Adventure Button) ���� ��ư ��Ȱ��ȭ
        select_side_button = side_buttons[0];
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
        if (select_side_button.button_index != button_index)
        {
            if (set_default_color != null) StopCoroutine(set_default_color);
            set_default_color = StartCoroutine(FadeImage(select_side_button.button_image, new Color(0.5f, 0.5f, 0.5f), true));

            for (int i = 0; i < side_buttons.Length; i++)
            {
                if (side_buttons[i].button_index == button_index)
                    select_side_button = side_buttons[i];
            }

            if (set_color != null) StopCoroutine(set_color);
            StartCoroutine(FadeImage(select_side_button.button_image, Color.white));
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

        // �� ��¥ �̳���ģ�� �� �ȵ�
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