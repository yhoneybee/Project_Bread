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
        public UnityEngine.Sprite not_startable;
        public UnityEngine.Sprite startable;
        public UnityEngine.Sprite one_star;
        public UnityEngine.Sprite two_star;
        public UnityEngine.Sprite three_star;
    }

    // 카메라 위치 제한 Transform 담아둔 구조체
    [SerializeField] LimitPostiions limits;

    // 게임 준비 창
    [SerializeField] GameObject ReadyWindow;

    // 배경에 배치되어 있는 각 Stage Object들
    [SerializeField] StageObject[] stage_objects;

    // 숫자 Text Sprite들
    [SerializeField] UnityEngine.Sprite[] font_1_text;
    [SerializeField] UnityEngine.Sprite[] font_2_text;

    // 왼쪽 상단 테마 이름 이미지
    [SerializeField] Image theme_name_txt_image;

    // Ready Window의 테마(시나리오) 텍스트 Sprite
    [SerializeField] UnityEngine.Sprite[] theme_name_sprites;

    // Ready Window의 Theme Number 이미지
    [SerializeField] Image theme_number_image;
    [SerializeField] Image[] stage_number_image = new Image[2];

    // Ready Window의 테마(시나리오) 텍스트 이미지
    [SerializeField] Image theme_name_image;

    // 스테이지 클리어 여부에 관련된 각각의 Sprite들
    [SerializeField] StageSprites stage_sprites;

    RaycastHit2D[] hits;
    Vector3 mouse_position => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    void Start()
    {
        // stage sprite 임시로 담아두는 배열
        UnityEngine.Sprite[] stage_sprite = { stage_sprites.startable, stage_sprites.one_star, stage_sprites.two_star, stage_sprites.three_star };
        // stage number text sprite 임시로 담아수는 배열
        UnityEngine.Sprite[] stage_text_sprites = { null, null };

        for (int i = 0; i < stage_objects.Length; i++)
        {
            stage_objects[i].is_startable = StageManager.Instance.GetStage(i).is_startable;
            // 스테이지 Sprite 변경
            stage_objects[i].SetObjectSprite(stage_objects[i].is_startable ? stage_sprite[StageManager.Instance.GetStage(i).star_count] : stage_sprites.not_startable);

            // 10의 자리 이하라면 해당 스테이지 번호를, 이상이라면 1 넣어줌
            stage_text_sprites[0] = i < 9 ? font_1_text[i + 1] : font_1_text[1];
            // 10의 자리 이하라면 null을, 이상이라면 0을 넣어줌 (스테이지 번호는 최대 10이기 때문에..)
            stage_text_sprites[1] = i < 9 ? null : font_1_text[0];

            stage_objects[i].SetTexts(font_1_text[StageInfo.theme_number], stage_text_sprites);
        }

        theme_name_txt_image.sprite = theme_name_image.sprite = theme_name_sprites[StageInfo.theme_number - 1];
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse_down_position = mouse_position;

            hits = Physics2D.RaycastAll(mouse_down_position, Vector3.forward, 10);

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
                CameraMove(mouse_down_position);
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
        {
            CameraManager.Instance.MoveCamera(cam_move_position, 5);
        }
    }
    void OnReadyWindow()
    {
        StageInfo.stage_number = hits[0].transform.GetComponent<StageObject>().stage_number;

        ReadyWindow.SetActive(true);

        theme_number_image.sprite = font_2_text[StageInfo.theme_number];

        // stage number가 10의 자리로 인한 두개 이미지 사용
        if (StageInfo.stage_number >= 10)
        {
            stage_number_image[1].gameObject.SetActive(true);
            stage_number_image[0].sprite = font_2_text[1];
            stage_number_image[1].sprite = font_2_text[0];
        }
        else
        {
            stage_number_image[1].gameObject.SetActive(false);
            stage_number_image[0].sprite = font_2_text[StageInfo.stage_number];
        }
    }
}
