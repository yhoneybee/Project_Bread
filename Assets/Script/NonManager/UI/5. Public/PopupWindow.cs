using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupWindow : MonoBehaviour
{
    Animator animator = null;
    void Start()
    {
        if (!animator)
            animator = GetComponent<Animator>();
    }

    public void ClosePopupWindow()
    {
        animator.SetTrigger("Active Off");
        Invoke(nameof(ActiveOff), 1);
    }

    void ActiveOff()
    {
        if (name.Contains("Window"))
            gameObject.SetActive(false);
        else
            transform.parent.gameObject.SetActive(false);
    }
}
