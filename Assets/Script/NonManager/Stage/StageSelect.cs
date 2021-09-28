using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] LimitPostiions limits;
    [SerializeField] GameObject ReadyWindow;
    [SerializeField] GameObject StageWindow;

    RaycastHit2D[] hits;
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 cam_move_position = mouse_position;

            // limits와 카메라 위치 비교 후 제한
            if (cam_move_position.x < limits.limit_x_left.position.x) cam_move_position = new Vector2(limits.limit_x_left.position.x, cam_move_position.y);
            else if (cam_move_position.x > limits.limit_x_right.position.x) cam_move_position = new Vector2(limits.limit_x_right.position.x, cam_move_position.y);

            if (cam_move_position.y < limits.limit_y_under.position.y) cam_move_position = new Vector2(cam_move_position.x, limits.limit_y_under.position.y);
            else if (cam_move_position.y > limits.limit_y_high.position.y) cam_move_position = new Vector2(cam_move_position.x, limits.limit_y_high.position.y);

            // 창이 열려 있지 않을 경우 카메라 이동
            if (!ReadyWindow.activeSelf && !StageWindow.activeSelf)
                CameraManager.Instance.MoveCamera(cam_move_position, 5);

            hits = Physics2D.RaycastAll(mouse_position, Vector3.forward, 10);

            // 충돌된 오브젝트가 있을 때 검사
            if (hits.Length > 0)
            {
                if (hits[0].transform.CompareTag("Stage Sprite"))
                {
                    // 스테이지 번호 갖고 있는 오브젝트
                    Transform stage_number_transform = hits[0].transform.GetChild(4);
                    SpriteRenderer[] number_images = new SpriteRenderer[2];

                    // 두 글자 이상일 때 (10스테이지 이상)
                    if (stage_number_transform.childCount > 0)
                    {
                        number_images = stage_number_transform.GetComponentsInChildren<SpriteRenderer>();
                    }
                    else
                    {
                        number_images[0] = stage_number_transform.GetComponent<SpriteRenderer>();
                    }

                    string stage_number = "";
                    foreach (var image in number_images)
                    {
                        if (image != null)
                            stage_number += image.sprite.name;
                    }
                    StageInfo.stage_number = System.Convert.ToInt32(stage_number);

                    ReadyWindow.SetActive(true);
                }
            }
            else
            {
                ReadyWindow.SetActive(false);
                StageWindow.SetActive(false);
            }
        }
    }
}