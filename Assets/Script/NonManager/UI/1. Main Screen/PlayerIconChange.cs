using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIconChange : MonoBehaviour
{
    public Image imgIcon;
    public RectTransform rtrnIconsParent;
    public GameObject goOrigin;
    public GameObject goConfirm;
    public Image before;
    public Image after;

    private Sprite sprite;

    private void OnEnable()
    {
        foreach (var unit in UnitManager.Instance.Units)
        {
            if (unit.Info.Gotten)
            {
                var Obj = Instantiate(goOrigin, rtrnIconsParent, false);
                var img = Obj.transform.GetChild(0).GetComponent<Image>();
                img.sprite = unit.Info.Icon;
                UIManager.Instance.FixSizeToRatio(img, rtrnIconsParent.GetComponent<GridLayoutGroup>().cellSize.x);
                Obj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    goConfirm.SetActive(true);
                    sprite = img.sprite;
                    before.sprite = imgIcon.sprite;
                    var rtrnBefore = before.GetComponent<RectTransform>();
                    UIManager.Instance.FixSizeToRatio(before, rtrnBefore.sizeDelta.y);
                    after.sprite = sprite;
                    var rtrnAfter = after.GetComponent<RectTransform>();
                    UIManager.Instance.FixSizeToRatio(after, rtrnAfter.sizeDelta.y);
                });
            }
        }
    }

    void Start()
    {
        goConfirm.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => 
        {
            imgIcon.sprite = sprite;
            var rtrnImg = imgIcon.GetComponent<RectTransform>();
            UIManager.Instance.FixSizeToRatio(imgIcon, rtrnImg.sizeDelta.y);
            rtrnImg.anchoredPosition = Vector2.zero;
        });
    }

    void Update()
    {
    }
}
