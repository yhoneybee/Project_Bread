using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ī�޶� ����� ���õ� Ŭ����
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; } = null;
    private new Camera camera;

    Coroutine CameraMove = null;
    Vector3 target_position;
    float move_speed;


    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
    }
    public void SetCamera(Camera camera)
    {
        this.camera = camera;
    }
    /// <summary>
    /// Vector2.Lerp�� �� �� ī�޶� �̵���Ű�� �Լ�
    /// </summary>
    /// <param name="position">ī�޶� �̵���ų ��ġ</param>
    /// <param name="speed">ī�޶� �̵� �üӵ�</param>
    public void MoveCamera(Vector3 position, float speed)
    {
        target_position = position;
        move_speed = speed;

        if (CameraMove == null) CameraMove = StartCoroutine(_MoveCamera());
        else
        {
            StopCoroutine(CameraMove);
            CameraMove = StartCoroutine(_MoveCamera());
        }
    }

    IEnumerator _MoveCamera()
    {
        float distance = Vector2.Distance(camera.transform.position, target_position);
        while (distance > 0.05f)
        {
            yield return new WaitForSeconds(0.01f);
            camera.transform.position = Vector3.Lerp(camera.transform.position, target_position, Time.deltaTime * move_speed);
            distance = Vector2.Distance(camera.transform.position, target_position);
        }
        camera.transform.position = target_position;
    }
}
