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
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                if (btn.gameObject.name == "Back") SoundManager.Instance.Play("SFX/Button/Back Button");
                else SoundManager.Instance.Play("SFX/Button/Button Click");
            });
        }
    }
}
