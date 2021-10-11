using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject : MonoBehaviour
{
    public Transform number_object_transform;

    [SerializeField] SpriteRenderer theme_number_image;
    [SerializeField] SpriteRenderer[] stage_number_image;

    new SpriteRenderer renderer { get => GetComponent<SpriteRenderer>(); }

    public bool is_startable { get; set; }

    private void Start()
    {
    }

    /// <summary>
    /// 본인의 Stage Sprite를 바꿔주는 함수
    /// </summary>
    /// <param name="sprite">매개 변수로 받은 sprite로 본인 Stage Sprite를 바꿔줌</param>
    public void SetObjectSprite(Sprite sprite)
        => renderer.sprite = sprite;

    /// <summary>
    /// Text Object들의 이미지를 바꿔주는 함수
    /// </summary>
    /// <param name="theme">theme text object의 이미지</param>
    /// <param name="stage">stage text object의 이미지 (1~2의 크기)</param>
    public void SetTexts(Sprite theme, Sprite[] stage)
    {
        theme_number_image.sprite = theme;

        for (int i = 0; i < 2; i++)
            if (stage[i] != null) stage_number_image[i].sprite = stage[i];
    }
}
