using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnToggleSwitchSprite : MonoBehaviour
{
    public Image img;
    public Toggle toggle;
    public Sprite onSprite;
    public Sprite offSprite;
    public void ChangeToggle() => img.sprite = toggle.isOn ? onSprite : offSprite;
}
