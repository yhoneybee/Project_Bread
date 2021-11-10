using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamBtnLock : MonoBehaviour
{
    public Image Image;
    public Button Button;
    public Sprite Sprite;
    public Sprite OverSprite;
    public int index;

    bool locked;

    private void Update()
    {
        if (!locked)
        {
            if (index == GameManager.Instance.Index) Image.sprite = OverSprite;
            else Image.sprite = Sprite;
        }
    }

    public void Lock()
    {
        locked = true;
        Sprite = Image.sprite;
        Image.sprite = UIManager.Instance.TeamBtnLock;
        Button.enabled = false;
    }
    public void UnLock()
    {
        locked = false;
        Image.sprite = Sprite;
        Button.enabled = true;
    }
}
