using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    private void Start()
    {
        foreach (var btn in GetComponentsInChildren<Button>())
        {
            btn.onClick.AddListener(() =>
            {
                if (btn.gameObject.name == "Back") SoundManager.Instance.Play("SFX/Button/Back Button");
                else SoundManager.Instance.Play("SFX/Button/Button Click");
            });
        }
    }

    public void BtnClick(bool isBack)
    {
        if (isBack) SoundManager.Instance.Play("SFX/Button/Back Button");
        else SoundManager.Instance.Play("SFX/Button/Button Click");
    }
}
