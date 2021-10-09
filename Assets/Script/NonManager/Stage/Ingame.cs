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
    }

    [SerializeField] TextMeshProUGUI stage_name_text;
    [SerializeField] TextMeshProUGUI theme_name_text;

    [Space(10)]
    [SerializeField] GameObject[] backgrounds;
    [SerializeField] GameObject[] platforms;

    [Space(10)]
    [SerializeField] Result_Window result_window;

    [Space(10)]
    [SerializeField] Unit friendly_tower;
    [SerializeField] Unit enemy_tower;


    [Space(10)]
    [SerializeField] Slider guage_slider;
    [SerializeField] Text guage_text;

    [Space(10)]
    [SerializeField] List<GameObject> card_units;

    private List<Image> image_blinds = new List<Image>();
    private List<Text> image_cost_texts = new List<Text>();

    private WaveInformation wave_data;

    Coroutine EnemySpawn = null;
    Coroutine GuageChange = null;

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

        wave_data = StageManager.Instance.GetWaveData();

        EnemySpawn = StartCoroutine(SpawnEnemies());
        GuageChange = StartCoroutine(Guage_Change());
    }
    void Update()
    {
        // 임시로 만들어 놓은 부분
        if (Input.GetKeyDown(KeyCode.Return))
        {
            current_guage--;
            target_guage--;
        }

        Check_Game_End();

        Set_Unit_Interfaces();

        guage_slider.value = current_guage / 10;
        guage_text.text = ((int)current_guage).ToString();
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

            result_window.result_window.SetActive(true);
            result_window.reward_window.SetActive(is_game_clear);
            result_window.title_text.text = is_game_clear ? "게임 클리어!" : "게임 오버..";
            result_window.button_text.text = is_game_clear ? "다음\n스테이지" : $"{StageInfo.theme_number} - {StageInfo.stage_number}\n재시작";

            foreach (var unit in (is_game_clear ? enemy_tower : friendly_tower).transform.GetComponentsInChildren<Unit>())
            {
                Destroy(unit);
            }

            result_window.button_text.GetComponentInParent<Button>().onClick.AddListener(
                () =>
                {
                    StageInfo.stage_number += is_game_clear ? 1 : 0;
                    ButtonActions.Instance.ChangeScene("E - 01 DeckView");
                });
        }
    }

    public void SetTimeScale(float value)
    {
        Time.timeScale = value;
    }
    /// <summary>
    /// 아군 빵을 생성하는 함수
    /// </summary>
    /// <param name="index">눌린 버튼(Card)의 index</param>
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
            // wave_data의 wave_information에서 담겨 있는 유닛 정보와 딜레이를 이용하여 웨이브 구성
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
            // 현재 게이지가 거의 다 올라갔을 때 (Lerp를 통한 값 설정으로 인해 정확히 올라가지 못하기 때문)
            if (target_guage - current_guage <= 0.05f)
            {
                // 정확한 값으로 설정 및 target_guage 설정
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