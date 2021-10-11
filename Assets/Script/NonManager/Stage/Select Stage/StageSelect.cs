using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [System.Serializable]
    struct StageSprites
    {
        public Sprite not_startable;
        public Sprite startable;
        public Sprite one_star;
        public Sprite two_star;
        public Sprite three_star;
    }

    /// <summary> ī�޶� �̵� ��ġ ���� ���� ��� ����ü </summary>///
    [SerializeField] LimitPostiions limits;
    /// <summary> ���� �غ� â </summary>///
    [SerializeField] GameObject ReadyWindow;
    [SerializeField] Image[] enemies_image;
    [SerializeField] Image[] rewards_image;

    /// <summary> �� 10�� �������� ������Ʈ�� </summary>///
    [SerializeField] StageObject[] stage_objects;
    /// <summary> ����(0~9) �ؽ�Ʈ Sprite </summary>///
    [SerializeField] Sprite[] text_number_sprites;
    /// <summary> 스테이지 클리어 여부에 관련된 각각의 Sprite들 </summary>///
    [SerializeField] StageSprites stage_sprites;
    [Space(15)]

    /// <summary> �������� ���� â���� �������� �̸� �ؽ�Ʈ </summary>///
    [SerializeField] TextMeshProUGUI stage_name_text;
    /// <summary> �������� ���� â���� �׸� �̸� �ؽ�Ʈ </summary>///
    [SerializeField] TextMeshProUGUI theme_name_text;

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

            // �浹�� ������Ʈ�� ���� �� �˻�
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
                enemies_image[i].sprite = enemies_sprite[i];
                enemies_image[i].SetNativeSize();
                enemies_image[i].GetComponent<RectTransform>().sizeDelta /= 5;
            }

        Sprite[] rewards_sprite = StageManager.Instance.GetRewardsSprite();

        for (int i = 0; i < rewards_sprite.Length; i++)
            if (rewards_sprite[i])
                rewards_image[i].sprite = rewards_sprite[i];
    }
}
