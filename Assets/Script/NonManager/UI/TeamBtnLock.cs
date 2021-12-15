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

    private void Start()
    {
        Button.onClick.AddListener(() => 
        {
            if (locked)
            {
                UIManager.Instance.ReleaseUI.go.SetActive(true);
                UIManager.Instance.ReleaseUI.tmpResource.color = GameManager.Instance.Coin >= 50000 ? Color.white : Color.red;
            }
        });
    }

    private void Update()
    {
        if (!locked)
        {
            if (index - 1 == GameManager.Instance.Index) Image.sprite = OverSprite;
            else Image.sprite = Sprite;
        }
    }

    public void Lock()
    {
        locked = true;
        Image.sprite = UIManager.Instance.TeamBtnLock;
    }
    public void UnLock()
    {
        locked = false;
        Image.sprite = Sprite;
    }
}
