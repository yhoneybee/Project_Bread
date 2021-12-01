using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Zoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isHover;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isHover) isHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isHover) isHover = false;
    }

    private void Update()
    {
        if (isHover && Input.touchCount > 1)
        {

        }
    }
}
