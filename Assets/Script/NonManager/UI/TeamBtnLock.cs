using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamBtnLock : MonoBehaviour
{
    public Image Image;
    public Button Button;
    public Sprite Sprite;

    public void Lock()
    {
        Sprite = Image.sprite;
        Image.sprite = UIManager.Instance.TeamBtnLock;
        Button.enabled = false;
    }
    public void UnLock()
    {
        Image.sprite = Sprite;
        Button.enabled = true;
    }
}
