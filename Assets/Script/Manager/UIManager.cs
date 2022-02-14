using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct DailyUI
{
    public RectTransform Daily;
    public List<DailyRewardLinker> LinkerList;
    public RectTransform GetDailyParent;
    public DailyRewardLinker GetDaily;
}
[System.Serializable]
public struct SwitchSprite
{
    public Sprite ASprite;
    public Sprite BSprite;
}

[System.Serializable]
public struct ReleaseUI
{
    public GameObject go;
    public Button btnYes;
    public TextMeshProUGUI tmpResource;
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
    public List<Sprite> spRanks;
    public List<Sprite> spRankBgs;
    public Sprite[] Nums = new Sprite[10];
    public Sprite TeamBtnLock;
    public Sprite UnitNullSprite;
    public Sprite[] spIcon;
    public UnitUILinker Except;
    public Image Fade;
    public RectTransform Content;
    public GameObject SquadPrefab;
    public Image AnimImg;
    public TextMeshProUGUI txtNoStemina;
    public Button btnIcon;
    public RectTransform rtrnIconSelect;

    public Sprite[] IconSprites = new Sprite[4];

    public SettingWindowLinker setting_linker;

    public SwitchSprite ArrowSwitchSprite;
    public SwitchSprite EnquipSwitchSprite;
    public ReleaseUI ReleaseUI;
    public DailyUI DailyUI;
    public RewardInfoLinker rewardInfoLinker;

    int AnimIndex = 0;
    float time = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (ReleaseUI.go)
        {
            ReleaseUI.btnYes.onClick.AddListener(() =>
            {
                if (GameManager.Instance.Coin >= 50000)
                {
                    GameManager.Instance.Coin -= 50000;
                    DeckManager.Instance.AddSlot();
                }
            });
        }
        if (btnIcon)
        {
            btnIcon.onClick.AddListener(() =>
            {
                if (rtrnIconSelect) rtrnIconSelect.gameObject.SetActive(true);
            });
        }
        if (!ButtonActions.Instance.CheckReEntering("A-Loading"))
        {
            Fade.color = Color.black;
            Fade.raycastTarget = true;
            StartCoroutine(GameManager.Instance.EHideUI(Fade));
        }
        if (ButtonActions.Instance.CheckReEntering("D-02_UnitSelect"))
        {
            var btnObj = GameObject.Find("Deck Setting Button").GetComponent<Button>();
            if (btnObj) btnObj.gameObject.SetActive(ButtonActions.directMain);

            var friends = UnitManager.Instance.Units.FindAll((o) => { return o.UnitType == UnitType.FRIEND && o.Info.Gotten; });
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
                view.gameObject.SetActive(find == null || ButtonActions.directMain);
                view.Show = show;
            }

            var select_unit = GameManager.SelectUnit;

            if (select_unit && Except && !ButtonActions.directMain)
            {
                Except.gameObject.SetActive(true);
                Except.Viewer.Show = select_unit;
                Except.Icon.sprite = select_unit.Info.Icon;
                var find = AllUnits.Find((o) => { return o.Show == select_unit; });
                if (find) find.gameObject.SetActive(false);
            }
        }
        else if (ButtonActions.Instance.CheckReEntering("D-04_UnitInfo") || ButtonActions.Instance.CheckReEntering("B-Main"))
        {
            AnimState = AnimState.WALK;
        }

        SetVolumeSliders();
    }
    private void Update()
    {
        if (ButtonActions.Instance.CheckReEntering("B-Main"))
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
                    FixSizeToRatio(AnimImg, 600);
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

    public void SetVolumeSliders()
    {
        if (setting_linker)
        {
            if (setting_linker.BGMSlider)
            {
                setting_linker.BGMSlider.onValueChanged.AddListener((v) => { SoundManager.Instance.BgmVolume = v; });
                setting_linker.BGMSlider.value = SoundManager.Instance.BgmVolume;
            }
            if (setting_linker.SFXSlider)
            {
                setting_linker.SFXSlider.onValueChanged.AddListener((v) => { SoundManager.Instance.SfxVolume = v; });
                setting_linker.SFXSlider.value = SoundManager.Instance.SfxVolume;
            }
        }
    }

    public void Animator()
    {
        if (!ButtonActions.Instance.CheckReEntering("D-04_UnitInfo") || !GameManager.SelectUnit.Anim) return;
        time += Time.deltaTime;

        switch (AnimState)
        {
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
    void Animation(List<Sprite> SF)
    {
        AnimImg.gameObject.SetActive(SF.Count != 0);
        if (SF.Count == 0) return;
        if (AnimImg)
        {
            AnimImg.sprite = SF[AnimIndex];
            FixSizeToRatio(AnimImg, ButtonActions.Instance.CheckReEntering("B-Main") ? 600 : 200);
        }
        if (time >= /*SF[AnimIndex].Frame*/0.05f)
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

    public IEnumerator EColoringUI<T>(T ui, Color change_color, float change_speed)
        where T : Graphic
    {
        var wait = new WaitForSeconds(0.0001f);

        while (Mathf.Abs(ui.color.r - change_color.r) +
            Mathf.Abs(ui.color.g - change_color.g) +
            Mathf.Abs(ui.color.b - change_color.b) +
            Mathf.Abs(ui.color.a - change_color.a) > 0.005f)
        {
            ui.color = Color.Lerp(ui.color, change_color, Time.deltaTime * change_speed);
            yield return wait;
        }
        ui.color = change_color;

        yield return null;
    }

    public IEnumerator EMovingUI<T>(T ui, Vector2 change_pos, float move_speed, bool isLerp = false)
        where T : Graphic
    {
        var wait = new WaitForSeconds(0.0001f);
        var uiRTf = ui.GetComponent<RectTransform>();

        while (Vector2.Distance(uiRTf.anchoredPosition, change_pos) > 0.1f)
        {
            if (isLerp) uiRTf.anchoredPosition = Vector2.Lerp(uiRTf.anchoredPosition, change_pos, Time.deltaTime * move_speed);
            else uiRTf.anchoredPosition = Vector2.MoveTowards(uiRTf.anchoredPosition, change_pos, Time.deltaTime * move_speed);
            yield return wait;
        }
        uiRTf.anchoredPosition = change_pos;

        yield return null;
    }
}
