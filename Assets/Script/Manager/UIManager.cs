using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct DailyUI
{
    public RectTransform Get;
    public TextMeshProUGUI GetRewardText;
    public Image GetRewardIcon;
    public Button GetRewardBtn;
    public TextMeshProUGUI RewardBtnText;
    public TextMeshProUGUI Fail;
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
    public Sprite TeamBtnLock;
    public Sprite UnitNullSprite;
    public UILinker Except;
    public RectTransform Content;
    public GameObject SquadPrefab;
    public Image AnimImg;

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
                Except.Icon.sprite = select_unit.Info.Icon;
                var find = AllUnits.Find((o) => { return o.Show == select_unit; });
                if (find) find.gameObject.SetActive(false);
            }
        }
        else if (ButtonActions.Instance.CheckReEntering("D - 04 UnitInfo"))
        {
            AnimState = AnimState.IDLE;
        }
    }
    private void Update()
    {
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
                if (AnimIndex == GameManager.SelectUnit.Anim.Idle.Count)
                    AnimIndex = 0;
                break;
            case AnimState.WALK:
                Animation(GameManager.SelectUnit.Anim.Walk);
                if (AnimIndex == GameManager.SelectUnit.Anim.Walk.Count)
                    AnimIndex = 0;
                break;
            case AnimState.HIT:
                Animation(GameManager.SelectUnit.Anim.Hit);
                if (AnimIndex == GameManager.SelectUnit.Anim.Hit.Count)
                {
                    AnimState = AnimState.IDLE;
                    AnimIndex = 0;
                }
                break;
            case AnimState.ATTACK:
                Animation(GameManager.SelectUnit.Anim.Attack);
                if (AnimIndex == GameManager.SelectUnit.Anim.Attack.Count)
                {
                    AnimState = AnimState.IDLE;
                    AnimIndex = 0;
                }
                break;
        }
    }
    void Animation(List<SpriteFrame> SF)
    {
        if (SF.Count == 0) return;
        if (AnimImg) AnimImg.sprite = SF[AnimIndex].Sprite;
        if (time >= SF[AnimIndex].Frame) ++AnimIndex;
    }
}
