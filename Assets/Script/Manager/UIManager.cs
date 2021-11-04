using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct DailyUI
{
    public RectTransform Daily;
    public List<DailyRewardLinker> LinkerList;
}
[System.Serializable]
public struct SwitchSprite
{
    public Sprite ASprite;
    public Sprite BSprite;
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } = null;

    private AnimState anim_state;
    public AnimState AnimState
    {
        get { return anim_state; }
        set
        {
            if (anim_state != value)
            {
                AnimIndex = 0;
                anim_state = value;
            }
        }
    }

    public List<UnitView> UnitViews;
    public List<TeamBtnLock> TeamBtnLocks;
    public List<UnitView> AllUnits;
    public List<RectTransform> ProductParents;
    public Sprite TeamBtnLock;
    public Sprite UnitNullSprite;
    public UILinker Except;
    public RectTransform Content;
    public GameObject SquadPrefab;
    public Image AnimImg;

    public Sprite[] IconSprites = new Sprite[4];

    public SwitchSprite ArrowSwitchSprite;
    public SwitchSprite EnquipSwitchSprite;

    public DailyUI DailyUI;

    int AnimIndex = 0;
    float time = 0;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (ButtonActions.Instance.CheckReEntering("D - 02 UnitSelect"))
        {
            var friends = UnitManager.Instance.Units.FindAll((o) => { return o.UnitType == UnitType.FRIEND; });
            Content.sizeDelta = new Vector2 { x = Content.sizeDelta.x, y = friends.Count / 5 * (375.1f + 58) };
            for (int i = 0; i < friends.Count; i++)
            {
                var view_go = Instantiate(SquadPrefab, Content, false);
                var view = view_go.GetComponent<UnitView>();
                var show = friends[i];
                var find = GameManager.Select.Find((o) =>
                {
                    if (o == null) return false;
                    return o.Info.Name == show.Info.Name;
                });
                view.gameObject.SetActive(find == null);
                view.Show = show;
            }

            var select_unit = GameManager.SelectUnit;

            if (select_unit && Except)
            {
                Except.gameObject.SetActive(true);
                Except.Viewer.Show = select_unit;
                //Except.Icon.sprite = select_unit.Info.Icon;
                var find = AllUnits.Find((o) => { return o.Show == select_unit; });
                if (find) find.gameObject.SetActive(false);
            }
        }
        else if (ButtonActions.Instance.CheckReEntering("D - 04 UnitInfo") || ButtonActions.Instance.CheckReEntering("B - Main"))
        {
            AnimState = AnimState.WALK;
        }
    }
    private void Update()
    {
        if (ButtonActions.Instance.CheckReEntering("B - Main"))
        {
            if (GameManager.Select[0])
            {
                if (GameManager.Select[0].Anim)
                {
                    AnimImg.gameObject.SetActive(true);
                    time += Time.deltaTime;
                    Animation(GameManager.Select[0].Anim.Walk);
                }
                else
                {
                    AnimImg.sprite = GameManager.Select[0].Info.Icon;
                }
            }
            else
            {
                AnimImg.gameObject.SetActive(false);
            }
        }
        else
            Animator();
    }

    public void Animator()
    {
        if (!ButtonActions.Instance.CheckReEntering("D - 04 UnitInfo") || !GameManager.SelectUnit.Anim) return;
        time += Time.deltaTime;

        switch (AnimState)
        {
            case AnimState.IDLE:
                Animation(GameManager.SelectUnit.Anim.Idle);
                break;
            case AnimState.WALK:
                Animation(GameManager.SelectUnit.Anim.Walk);
                break;
            case AnimState.HIT:
                Animation(GameManager.SelectUnit.Anim.Hit);
                break;
            case AnimState.ATTACK:
                Animation(GameManager.SelectUnit.Anim.Attack);
                break;
        }
    }
    void Animation(List<SpriteFrame> SF)
    {
        if (SF.Count == 0) return;
        if (AnimImg) AnimImg.sprite = SF[AnimIndex].Sprite;
        if (time >= SF[AnimIndex].Frame)
        {
            ++AnimIndex;
            time = 0;
        }
        if (AnimIndex == SF.Count) AnimIndex = 0;
    }
    void FixSizeToRatioForXAxis(Image fix, float to_x_size, float a_min_x = 0.5f, float a_min_y = 0.5f, float a_max_x = 0.5f, float a_max_y = 0.5f)
    {
        var fixRTf = fix.GetComponent<RectTransform>();

        fix.SetNativeSize();

        fixRTf.anchorMax = new Vector2(a_max_x, a_max_y);
        fixRTf.anchorMin = new Vector2(a_min_x, a_min_y);

        float div = to_x_size / fixRTf.sizeDelta.x;
        fixRTf.sizeDelta *= div;
    }
    void FixSizeToRatioForYAxis(Image fix, float to_y_size, float a_min_x = 0.5f, float a_min_y = 0.5f, float a_max_x = 0.5f, float a_max_y = 0.5f)
    {
        var fixRTf = fix.GetComponent<RectTransform>();

        fix.SetNativeSize();

        fixRTf.anchorMax = new Vector2(a_max_x, a_max_y);
        fixRTf.anchorMin = new Vector2(a_min_x, a_min_y);

        float div = to_y_size / fixRTf.sizeDelta.y;
        fixRTf.sizeDelta *= div;
    }
    public void FixSizeToRatio(Image fix, float to_size, float a_min_x = 0.5f, float a_min_y = 0.5f, float a_max_x = 0.5f, float a_max_y = 0.5f)
    {
        var fixRTf = fix.GetComponent<RectTransform>();

        fix.SetNativeSize();

        if (fixRTf.sizeDelta.x > fixRTf.sizeDelta.y)
        {
            FixSizeToRatioForXAxis(fix, to_size, a_min_x, a_min_y, a_max_x, a_max_y);
        }
        else
        {
            FixSizeToRatioForYAxis(fix, to_size, a_min_x, a_min_y, a_max_x, a_max_y);
        }
    }
}
