using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingWindowLinker : MonoBehaviour
{
    public Toggle SFXMutes;
    public Slider SFXSlider;
    public Toggle BGMMutes;
    public Slider BGMSlider;

    private void Start()
    {
        SoundManager.Instance.audioSources[((int)SoundType.EFFECT)].mute = SFXMutes.isOn;
        SoundManager.Instance.audioSources[((int)SoundType.BGM)].mute = BGMMutes.isOn;
    }

    private void Update()
    {
        // TODO : 사운드 매니저가 있네요?
        SoundManager.Instance.SfxVolume = SFXSlider.value;
        SoundManager.Instance.BgmVolume = BGMSlider.value;
    }
}
