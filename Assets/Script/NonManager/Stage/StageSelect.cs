using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            if (cam_move_position.x < limits.limit_x_left.position.x) cam_move_position = new Vector2(limits.limit_x_left.position.x, cam_move_position.y);
            else if (cam_move_position.x > limits.limit_x_right.position.x) cam_move_position = new Vector2(limits.limit_x_right.position.x, cam_move_position.y);

            if (cam_move_position.y < limits.limit_y_under.position.y) cam_move_position = new Vector2(cam_move_position.x, limits.limit_y_under.position.y);
            else if (cam_move_position.y > limits.limit_y_high.position.y) cam_move_position = new Vector2(cam_move_position.x, limits.limit_y_high.position.y);


            if (!ReadyWindow.activeSelf && !StageWindow.activeSelf)
                CameraManager.Instance.MoveCamera(cam_move_position, 5);

            hits = Physics2D.RaycastAll(mouse_position, Vector3.forward, 10);

            if (hits.Length > 0)
            {
                if (hits[0].transform.CompareTag("Stage Sprite"))
                {
                    ReadyWindow.SetActive(true);
                }
                // Do Something!
                // Do Something!
                // Do Something!
            }
            else
            {
                ReadyWindow.SetActive(false);
                StageWindow.SetActive(false);
            }

        }
    }
}
