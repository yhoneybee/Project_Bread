using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DailyRewardLinker : MonoBehaviour
{
    public RectTransform Bg;
    public RectTransform Icon;
    public RectTransform Count;
    public RectTransform Already;
    public Button Get;

    private bool is_get;
    public bool isGet
    {
        get { return is_get; }
        set
        {
            is_get = value;
            Already.gameObject.SetActive(is_get);
            GameManager.Instance.Gets[transform.GetSiblingIndex()] = is_get;
        }
    }


    private void Start()
    {
        isGet = GameManager.Instance.Gets[transform.GetSiblingIndex()];
        Get.onClick.AddListener(() => { if (!isGet) isGet = true; });
        var img = Icon.GetComponent<Image>();
        img.SetNativeSize();
        var glg = transform.parent.GetComponent<GridLayoutGroup>();
        var div = (glg.cellSize.y - 10) / Icon.sizeDelta.y;
        Icon.sizeDelta *= div;
    }
}
