using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StageSelect : MonoBehaviour
{
    [System.Serializable]
    struct LimitPostiions // 카메라 위치 제한 정보 담긴 구조체
    {
        public Transform limit_x_left;
        public Transform limit_x_right;
        public Transform limit_y_high;
        public Transform limit_y_under;
    }

    /// <summary> 카메라 이동 위치 제한 정보 담긴 구조체 </summary>///
    [SerializeField] LimitPostiions limits;
    /// <summary> 게임 준비 창 </summary>///
    [SerializeField] GameObject ReadyWindow;

    /// <summary> 총 10개 스테이지 오브젝트들 </summary>///
    [SerializeField] StageObject[] stage_objects;
    /// <summary> 숫자(0~9) 텍스트 Sprite </summary>///
    [SerializeField] Sprite[] theme_number_sprites;

    /// <summary> 스테이지 정보 창에서 스테이지 이름 텍스트 </summary>///
    [SerializeField] TextMeshProUGUI stage_name_text;
    /// <summary> 스테이지 정보 창에서 테마 이름 텍스트 </summary>///
    [SerializeField] TextMeshProUGUI theme_name_text;

    RaycastHit2D[] hits;

    SpriteRenderer[] theme_number_imgs = new SpriteRenderer[10];
    void Start()
    {
        for (int i = 0; i < stage_objects.Length; i++)
        {
            // Sprite Renderer로 되어 있는 각 스테이지 버튼의 테마 번호
            theme_number_imgs[i] = stage_objects[i].theme_number_image;

            stage_objects[i].star_count = StageManager.Instance.GetStage(i).star_count;
            stage_objects[i].is_startable = StageManager.Instance.GetStage(i).is_startable;
            theme_number_imgs[i].sprite = theme_number_sprites[StageInfo.theme_number - 1];
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            CameraMove(mouse_position);

            hits = Physics2D.RaycastAll(mouse_position, Vector3.forward, 10);

            // 충돌된 오브젝트가 있을 때 검사
            if (hits.Length > 0)
            {
                if (hits[0].transform.CompareTag("Stage Sprite"))
                {
                    if (hits[0].transform.GetComponent<StageObject>().is_startable)
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
    /// 카메라가 범위를 벗어나지 않으면서 목표 위치로 이동시키는 함수
    /// </summary>
    /// <param name="target_position">목표 위치</param>
    void CameraMove(Vector2 target_position)
    {
        Vector2 cam_move_position = target_position;

        // limits와 카메라 위치 비교 후 제한
        if (cam_move_position.x < limits.limit_x_left.position.x) cam_move_position = new Vector2(limits.limit_x_left.position.x, cam_move_position.y);
        else if (cam_move_position.x > limits.limit_x_right.position.x) cam_move_position = new Vector2(limits.limit_x_right.position.x, cam_move_position.y);

        if (cam_move_position.y < limits.limit_y_under.position.y) cam_move_position = new Vector2(cam_move_position.x, limits.limit_y_under.position.y);
        else if (cam_move_position.y > limits.limit_y_high.position.y) cam_move_position = new Vector2(cam_move_position.x, limits.limit_y_high.position.y);

        // 창이 열려 있지 않을 경우 카메라 이동
        if (!ReadyWindow.activeSelf)
            CameraManager.Instance.MoveCamera(cam_move_position, 5);
    }
    /// <summary>
    ///  Stage 정보창 등에서 스테이지 이름과 테마 이름을 설정해주고 Ready Window를 띄워주는 함수
    /// </summary>
    void OnReadyWindow()
    {
        // 스테이지 번호 갖고 있는 오브젝트
        Transform stage_number_transform = hits[0].transform.GetChild(4);
        SpriteRenderer[] number_images;

        number_images = stage_number_transform.GetComponentsInChildren<SpriteRenderer>();

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
    }
}