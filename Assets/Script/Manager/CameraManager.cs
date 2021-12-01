using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카메라 연출과 관련된 클래스
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
    /// Vector2.Lerp를 통 해 카메라를 이동시키는 함수
    /// </summary>
    /// <param name="position">카메라를 이동시킬 위치</param>
    /// <param name="speed">카메라 이동 시속도</param>
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
