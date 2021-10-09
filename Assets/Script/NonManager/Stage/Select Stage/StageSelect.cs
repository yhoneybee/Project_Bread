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

    /// <summary> ī�޶� �̵� ��ġ ���� ���� ��� ����ü </summary>///
    [SerializeField] LimitPostiions limits;
    /// <summary> ���� �غ� â </summary>///
    [SerializeField] GameObject ReadyWindow;

    /// <summary> �� 10�� �������� ������Ʈ�� </summary>///
    [SerializeField] StageObject[] stage_objects;
    /// <summary> ����(0~9) �ؽ�Ʈ Sprite </summary>///
    [SerializeField] Sprite[] theme_number_sprites;

    /// <summary> �������� ���� â���� �������� �̸� �ؽ�Ʈ </summary>///
    [SerializeField] TextMeshProUGUI stage_name_text;
    /// <summary> �������� ���� â���� �׸� �̸� �ؽ�Ʈ </summary>///
    [SerializeField] TextMeshProUGUI theme_name_text;

    RaycastHit2D[] hits;

    SpriteRenderer[] theme_number_imgs = new SpriteRenderer[10];
    void Start()
    {
        for (int i = 0; i < stage_objects.Length; i++)
        {
            // Sprite Renderer�� �Ǿ� �ִ� �� �������� ��ư�� �׸� ��ȣ
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

            // �浹�� ������Ʈ�� ���� �� �˻�
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
    /// ī�޶� ������ ����� �����鼭 ��ǥ ��ġ�� �̵���Ű�� �Լ�
    /// </summary>
    /// <param name="target_position">��ǥ ��ġ</param>
    void CameraMove(Vector2 target_position)
    {
        Vector2 cam_move_position = target_position;

        // limits�� ī�޶� ��ġ �� �� ����
        if (cam_move_position.x < limits.limit_x_left.position.x) cam_move_position = new Vector2(limits.limit_x_left.position.x, cam_move_position.y);
        else if (cam_move_position.x > limits.limit_x_right.position.x) cam_move_position = new Vector2(limits.limit_x_right.position.x, cam_move_position.y);

        if (cam_move_position.y < limits.limit_y_under.position.y) cam_move_position = new Vector2(cam_move_position.x, limits.limit_y_under.position.y);
        else if (cam_move_position.y > limits.limit_y_high.position.y) cam_move_position = new Vector2(cam_move_position.x, limits.limit_y_high.position.y);

        // â�� ���� ���� ���� ��� ī�޶� �̵�
        if (!ReadyWindow.activeSelf)
            CameraManager.Instance.MoveCamera(cam_move_position, 5);
    }
    /// <summary>
    ///  Stage ����â ��� �������� �̸��� �׸� �̸��� �������ְ� Ready Window�� ����ִ� �Լ�
    /// </summary>
    void OnReadyWindow()
    {
        // �������� ��ȣ ���� �ִ� ������Ʈ
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