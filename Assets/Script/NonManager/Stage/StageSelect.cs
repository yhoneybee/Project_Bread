using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StageSelect : MonoBehaviour
{
    [System.Serializable]
    struct LimitPostiions // ī�޶� ��ġ ���� ���� ��� ����ü
    {
        public Transform limit_x_left;
        public Transform limit_x_right;
        public Transform limit_y_high;
        public Transform limit_y_under;
    }

    [SerializeField] LimitPostiions limits;
    [SerializeField] GameObject ReadyWindow;
    [SerializeField] GameObject StageWindow;

    [SerializeField] SpriteRenderer[] theme_number_imgs;
    [SerializeField] Sprite[] theme_number_sprites;

    [SerializeField] TextMeshProUGUI[] stage_name_texts;
    [SerializeField] TextMeshProUGUI[] theme_name_texts;

    RaycastHit2D[] hits;
    void Start()
    {
        for (int i = 0; i < theme_number_imgs.Length; i++)
        {
            // Sprite Renderer�� �Ǿ� �ִ� �� �������� ��ư�� �׸� ��ȣ
            theme_number_imgs[i].sprite = theme_number_sprites[StageInfo.theme_number - 1];
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("F - 01 Ingame");
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 cam_move_position = mouse_position;

            // limits�� ī�޶� ��ġ �� �� ����
            if (cam_move_position.x < limits.limit_x_left.position.x) cam_move_position = new Vector2(limits.limit_x_left.position.x, cam_move_position.y);
            else if (cam_move_position.x > limits.limit_x_right.position.x) cam_move_position = new Vector2(limits.limit_x_right.position.x, cam_move_position.y);

            if (cam_move_position.y < limits.limit_y_under.position.y) cam_move_position = new Vector2(cam_move_position.x, limits.limit_y_under.position.y);
            else if (cam_move_position.y > limits.limit_y_high.position.y) cam_move_position = new Vector2(cam_move_position.x, limits.limit_y_high.position.y);

            // â�� ���� ���� ���� ��� ī�޶� �̵�
            if (!ReadyWindow.activeSelf && !StageWindow.activeSelf)
                CameraManager.Instance.MoveCamera(cam_move_position, 5);

            hits = Physics2D.RaycastAll(mouse_position, Vector3.forward, 10);

            // �浹�� ������Ʈ�� ���� �� �˻�
            if (hits.Length > 0)
            {
                if (hits[0].transform.CompareTag("Stage Sprite"))
                {
                    // �������� ��ȣ ���� �ִ� ������Ʈ
                    Transform stage_number_transform = hits[0].transform.GetChild(4);
                    SpriteRenderer[] number_images;

                    // �� ���� �̻��� �� (10�������� �̻�)
                    number_images = stage_number_transform.GetComponentsInChildren<SpriteRenderer>();

                    string stage_number = "";
                    foreach (var image in number_images)
                    {
                        if (image != null)
                            stage_number += image.sprite.name;
                    }
                    StageInfo.stage_number = System.Convert.ToInt32(stage_number);

                    ReadyWindow.SetActive(true);

                    foreach (var theme_name_text in theme_name_texts)
                    {
                        theme_name_text.text = StageInfo.theme_name;
                    }
                    foreach (var stage_name_text in stage_name_texts)
                    {
                        stage_name_text.text = $"{StageInfo.theme_number} - {StageInfo.stage_number}";
                    }
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