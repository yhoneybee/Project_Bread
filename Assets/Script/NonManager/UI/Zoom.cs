using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Zoom : MonoBehaviour, IPointerEnterHandler
{
    public bool IsHover
    {
        get => isHover;
        set => isHover = value;
    }

    [SerializeField] private Image imgZooming;
    [SerializeField] private bool isHover;
    private List<Vector2> v2Touchs;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isHover) isHover = true;
    }

    private void Start()
    {
        v2Touchs = new List<Vector2>();
        v2Touchs.Add(Vector2.zero);
        v2Touchs.Add(Vector2.zero);
    }

    private void Update()
    {
        if (isHover && Input.touchCount > 1 && Input.GetTouch(0).deltaPosition != Vector2.zero && Input.GetTouch(1).deltaPosition != Vector2.zero)
        {
            for (int i = 0; i < v2Touchs.Count; i++)
            {
                if (v2Touchs[i] == Vector2.zero)
                    v2Touchs[i] = Input.GetTouch(i).position;
            }
            float startDis = Vector2.Distance(v2Touchs[0], v2Touchs[1]);
            float dragDis = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
            imgZooming.GetComponent<RectTransform>().sizeDelta += Vector2.one * (dragDis - startDis) * 0.05f;
        }
        if (Input.touchCount == 0)
        {
            isHover = false;
            for (int i = 0; i < v2Touchs.Count; i++)
            {
                v2Touchs[i] = Vector2.zero;
            }
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).deltaPosition == Vector2.zero)
        {
            v2Touchs[0] = Input.GetTouch(0).position;
        }
        if (Input.touchCount > 1 && Input.GetTouch(1).deltaPosition == Vector2.zero)
        {
            v2Touchs[1] = Input.GetTouch(1).position;
        }
    }
}
