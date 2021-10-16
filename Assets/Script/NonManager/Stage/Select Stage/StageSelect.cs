using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StageSelect : MonoBehaviour
{
    [System.Serializable]
    struct LimitPostiions
    {
        public Transform limit_x_left;
        public Transform limit_x_right;
        public Transform limit_y_high;
        public Transform limit_y_under;
    }
    [System.Serializable]
    struct StageSprites
    {
        public Sprite not_startable;
        public Sprite startable;
        public Sprite one_star;
        public Sprite two_star;
        public Sprite three_star;
    }

    // 카메라 위치 제한 Transform 담아둔 구조체
    [SerializeField] LimitPostiions limits;

    // 게임 준비 창
    [SerializeField] GameObject ReadyWindow;

    // ReadyWindow의 stage name Text
    [SerializeField] TextMeshProUGUI stage_name_text;

    // ReadyWindow의 theme name Text
    [SerializeField] TextMeshProUGUI theme_name_text;

    // 배경에 배치되어 있는 각 Stage Object들
    [SerializeField] StageObject[] stage_objects;

    // 숫자 Text Sprite들
    [SerializeField] Sprite[] text_number_sprites;

    // 스테이지 클리어 여부에 관련된 각각의 Sprite들
    [SerializeField] StageSprites stage_sprites;

    RaycastHit2D[] hits;

    void Start()
    {
        // stage sprite 임시로 담아두는 배열
        Sprite[] stage_sprite = { stage_sprites.startable, stage_sprites.one_star, stage_sprites.two_star, stage_sprites.three_star };
        // stage number text sprite 임시로 담아수는 배열
        Sprite[] stage_text_sprites = { null, null };

        for (int i = 0; i < stage_objects.Length; i++)
        {
            stage_objects[i].is_startable = StageManager.Instance.GetStage(i).is_startable;
            // 스테이지 Sprite 변경
            stage_objects[i].SetObjectSprite(stage_objects[i].is_startable ? stage_sprite[StageManager.Instance.GetStage(i).star_count] : stage_sprites.not_startable);

            // 10의 자리 이하라면 해당 스테이지 번호를, 이상이라면 1 넣어줌
            stage_text_sprites[0] = i < 9 ? text_number_sprites[i + 1] : text_number_sprites[1];
            // 10의 자리 이하라면 null을, 이상이라면 0을 넣어줌 (스테이지 번호는 최대 10이기 때문에..)
            stage_text_sprites[1] = i < 9 ? null : text_number_sprites[0];

            stage_objects[i].SetTexts(text_number_sprites[StageInfo.theme_number], stage_text_sprites);
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            CameraMove(mouse_position);

            hits = Physics2D.RaycastAll(mouse_position, Vector3.forward, 10);

            if (hits.Length > 0)
            {
                if (hits[hits.Length - 1].transform.CompareTag("Stage Sprite"))
                {
                    // Stage Sprite만 감지 (위에 있는 UI를 클릭했을 때를 제외시키기 위함)
                    if (hits[hits.Length - 1].transform.GetComponent<StageObject>().is_startable)
                        OnReadyWindow();
                }
            }
            else
            {
                ReadyWindow.SetActive(false);
            }
        }
    }
    /// <summary>
    /// 카메라가 범위 내에서 움직일 수 있도록 이동시켜주는 함수
    /// </summary>
    /// <param name="target_position">카메라를 이동시킬 위치</param>
    void CameraMove(Vector2 target_position)
    {
        Vector2 cam_move_position = target_position;

        if (cam_move_position.x < limits.limit_x_left.position.x) cam_move_position = new Vector2(limits.limit_x_left.position.x, cam_move_position.y);
        else if (cam_move_position.x > limits.limit_x_right.position.x) cam_move_position = new Vector2(limits.limit_x_right.position.x, cam_move_position.y);

        if (cam_move_position.y < limits.limit_y_under.position.y) cam_move_position = new Vector2(cam_move_position.x, limits.limit_y_under.position.y);
        else if (cam_move_position.y > limits.limit_y_high.position.y) cam_move_position = new Vector2(cam_move_position.x, limits.limit_y_high.position.y);

        if (!ReadyWindow.activeSelf)
            CameraManager.Instance.MoveCamera(cam_move_position, 5);
    }
    void OnReadyWindow()
    {
        Transform stage_number_transform = hits[0].transform.GetComponent<StageObject>().number_object_transform;
        SpriteRenderer[] number_images = stage_number_transform.GetComponentsInChildren<SpriteRenderer>();

        string stage_number = "";
        foreach (var image in number_images)
        {
            if (image != null)
                stage_number += image.sprite.name;
        }
        StageInfo.stage_number = System.Convert.ToInt32(stage_number);

        ReadyWindow.SetActive(true);

        theme_name_text.text = StageInfo.theme_name;
        stage_name_text.text = $"{StageInfo.theme_number} - {StageInfo.stage_number}";

        Sprite[] enemies_sprite = StageManager.Instance.GetEnemiesSprite();

        for (int i = 0; i < enemies_sprite.Length; i++)
            if (enemies_sprite[i])
            {
            }

        Sprite[] rewards_sprite = StageManager.Instance.GetRewardsSprite();

    }
}
