using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioMuteToggle : MonoBehaviour
{
    public Image img;
    public Toggle toggle;
    public Sprite onSprite;
    public Sprite offSprite;
    public bool isBgm;
    public void ChangeToggle()
    {
        img.sprite = toggle.isOn ? onSprite : offSprite;
        if (isBgm) SoundManager.Instance.audioSources[((int)SoundType.BGM)].mute = toggle.isOn;
        else
        {
            for (int i = 0; i < SoundManager.Instance.audioSources.Length; i++)
            {
                if (i == (int)SoundType.BGM) continue;
                SoundManager.Instance.audioSources[i].mute = toggle.isOn;
            }
        }
    }
}
