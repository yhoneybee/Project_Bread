using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ingame : MonoBehaviour
{
    [System.Serializable]
    struct Result_Window
    {
        public GameObject result_window;
        public TextMeshProUGUI title_text;
        public GameObject reward_window;
        public TextMeshProUGUI button_text;
        public RectTransform stars_parent;
    }
    [System.Serializable]
    struct Three_Star_Limit
    {
        public Slider slider;
        public Image[] lines;
        public Image[] stars;
    }

    [SerializeField] TextMeshProUGUI stage_name_text;
    [SerializeField] TextMeshProUGUI theme_name_text;

    [Space(10)]
    [SerializeField] GameObject[] backgrounds;
    [SerializeField] GameObject[] platforms;

    [Space(10)]
    [SerializeField] Result_Window result_window;

    [Space(10)]
    [SerializeField] Three_Star_Limit three_star_limit;

    [Space(10)]
    [SerializeField] Unit friendly_tower;
    [SerializeField] Unit enemy_tower;

    [Space(10)]
    [SerializeField] Slider guage_slider;
    [SerializeField] Text guage_text;

    [Space(10)]
    [SerializeField] List<GameObject> card_units;

    [Space(10)]
    [SerializeField] StageInfoLinker stage_linker;

    private List<Image> image_blinds = new List<Image>();
    private List<Text> image_cost_texts = new List<Text>();

    private WaveInformation wave_data;

    Coroutine EnemySpawn = null;
    Coroutine GuageChange = null;

    int game_count = 0;
    int three_star_count = 120;
    int current_star_count = 3;

    float current_guage = 0;
    float target_guage = 1;
    bool is_game_clear = false;
    bool is_game_over = false;
    void Start()
    {
        stage_name_text.text = $"{StageInfo.theme_number} - {StageInfo.stage_number}";
        theme_name_text.text = StageInfo.theme_name;

        backgrounds[StageInfo.theme_number - 1].SetActive(true);
        platforms[StageInfo.theme_number - 1].SetActive(true);

        SetUIsSize();

        wave_data = StageManager.Instance.GetWaveData();

        EnemySpawn = StartCoroutine(SpawnEnemies());
        GuageChange = StartCoroutine(Guage_Change());

        InvokeRepeating(nameof(CountUp), 0, 1);
    }
    void Update()
    {
        Check_Game_End();

        Set_Unit_Interfaces();

        guage_slider.value = current_guage / 10;
        guage_text.text = ((int)current_guage).ToString();

        Set_Three_Star_Limit_UIs();
    }
    void SetUIsSize()
    {
        int card_index;
        Image card_image;
        foreach (var card_unit in card_units)
        {
            card_index = card_units.IndexOf(card_unit);
            if (DeckManager.Select[card_units.IndexOf(card_unit)])
            {
                card_image = card_unit.transform.GetChild(1).GetComponent<Image>();
                card_image.sprite = DeckManager.Select[card_index].Info.Icon;
                card_image.SetNativeSize();
                card_image.GetComponent<RectTransform>().sizeDelta /= DeckManager.Select[card_index].Info.DValue;
            }

            image_blinds.Add(card_unit.transform.GetChild(2).GetComponent<Image>());
            image_cost_texts.Add(card_unit.GetComponentInChildren<Text>());
        }
    }
    void CountUp()
    {
        game_count++;
    }
    void Set_Unit_Interfaces()
    {
        int deck_index;
        foreach (var card_unit in card_units)
        {
            deck_index = card_units.IndexOf(card_unit);
            if (DeckManager.Select[deck_index])
            {
                image_blinds[deck_index].fillAmount = 1 - current_guage / DeckManager.Select[deck_index].Info.Cost;
                image_cost_texts[deck_index].text = DeckManager.Select[deck_index].Info.Cost.ToString();
            }
        }
    }
    void Check_Game_End()
    {
        if (result_window.result_window.activeSelf) return;
        is_game_clear = enemy_tower.Stat.HP <= 0;
        is_game_over = friendly_tower.Stat.HP <= 0;

        if (is_game_clear || is_game_over)
        {
            StopCoroutine(EnemySpawn);
            StopCoroutine(GuageChange);

            CancelInvoke(nameof(CountUp));

            result_window.result_window.SetActive(true);
            result_window.reward_window.SetActive(is_game_clear);
            result_window.title_text.text = is_game_clear ? "게임 클리어!" : "게임 오버..";
            result_window.button_text.text = is_game_clear ? "다음\n스테이지" : $"{StageInfo.theme_number} - {StageInfo.stage_number}\n재시작";

            foreach (var unit in (is_game_clear ? enemy_tower : friendly_tower).transform.GetComponentsInChildren<Unit>())
            {
                Destroy(unit);
            }

            if (is_game_clear)
            {
                result_window.stars_parent.gameObject.SetActive(true);

                bool is_first_clear = StageManager.Instance.GetStage().star_count == 0;
                bool is_three_star_clear = current_star_count == 3 && StageManager.Instance.GetStage().star_count < 3;

                stage_linker.SetRewards(is_first_clear, is_three_star_clear);
                var rewards = stage_linker.GetRewards(is_first_clear, is_three_star_clear);

                GameManager.Instance.Coin += rewards.Item1;
                GameManager.Instance.Jem += rewards.Item2;

                if (current_star_count > StageManager.Instance.GetStage().star_count)
                    StageManager.Instance.GetStage().star_count = current_star_count;
                StageInfo.stage_number++;
                StageManager.Instance.GetStage().is_startable = true;

                Image[] star_images = result_window.stars_parent.GetComponentsInChildren<Image>();
                for (int i = 0; i < current_star_count; i++)
                {
                    star_images[i].color = Color.yellow;
                }
            }

            result_window.button_text.GetComponentInParent<Button>().onClick.AddListener(
                () =>
                {
                    ButtonActions.Instance.ChangeScene("E - 01 DeckView");
                });
        }
    }
    /// <summary>
    /// 3성 조건 UI들 설정해줌 (Slider value, Image Fade 등)
    /// </summary>
    void Set_Three_Star_Limit_UIs()
    {
        float value = 1 - game_count / (float)(three_star_count);
        three_star_limit.slider.value = value;

        int index;

        switch (three_star_limit.slider.value)
        {
            case float f when (f == 0f):
                index = 0;
                current_star_count = 1;
                foreach (var image in three_star_limit.slider.GetComponentsInChildren<Image>())
                    if (image.enabled)
                        FadeOutImage(image);
                break;
            case float f when (f < 0.33f):
                index = 1;
                current_star_count = 2;
                break;
            default:
                return;
        }

        // 이미지 fade out해야될 경우 해당 이미지를 fade out 처리
        if (three_star_limit.stars[index].enabled)
            FadeOutImage(three_star_limit.stars[index]);

        if (three_star_limit.lines[index].enabled)
            FadeOutImage(three_star_limit.lines[index]);
    }
    /// <summary>
    /// Image를 Fade Out 해주는 함수
    /// </summary>
    /// <param name="image">Fade Out 처리할 이미지</param>
    void FadeOutImage(Image image)
    {
        image.color = Color.Lerp(image.color, new Color(image.color.r, image.color.g, image.color.b, 0), 0.1f);
        image.enabled = image.color.a > 0.01f;
    }

    public void SetTimeScale(float value)
    {
        Time.timeScale = value;
    }
    /// <summary>
    /// �Ʊ� ���� �����ϴ� �Լ�
    /// </summary>
    /// <param name="index">���� ��ư(Card)�� index</param>
    public void SpawnBread(int index)
    {
        Unit unit = DeckManager.Select[index];

        if (current_guage < unit.Info.Cost) return;
        target_guage -= unit.Info.Cost;
        current_guage -= unit.Info.Cost;

        Unit bread = UnitManager.Instance.GetUnit(unit.Info.Name, friendly_tower.transform.position);
        bread.transform.SetParent(friendly_tower.transform);
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // wave_data�� wave_information���� ��� �ִ� ���� ������ ����̸� �̿��Ͽ� ���̺� ����
            foreach (var data in wave_data.wave_information)
            {
                if (data.unit != null)
                {
                    Unit enemy = UnitManager.Instance.GetUnit(data.unit.Info.Name, enemy_tower.transform.position);
                    enemy.transform.SetParent(enemy_tower.transform);
                }
                yield return new WaitForSeconds(data.delay);
            }
        }
    }
    IEnumerator Guage_Change()
    {
        while (true)
        {
            // ���� �������� ���� �� �ö��� �� (Lerp�� ���� �� �������� ���� ��Ȯ�� �ö��� ���ϱ� ����)
            if (target_guage - current_guage <= 0.05f)
            {
                // ��Ȯ�� ������ ���� �� target_guage ����
                current_guage = Mathf.Round(current_guage);
                if (current_guage < 10)
                    target_guage = current_guage + 1;
                yield return new WaitForSeconds(1f);
            }
            current_guage = Mathf.Lerp(current_guage, target_guage, 0.2f);

            yield return new WaitForSeconds(0.01f);
        }
    }
}