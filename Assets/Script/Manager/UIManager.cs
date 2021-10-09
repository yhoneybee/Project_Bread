using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } = null;

    public List<UnitView> UnitViews;
    public List<TeamBtnLock> TeamBtnLocks;
    public List<UnitView> AllUnits;
    public Sprite TeamBtnLock;
    public Sprite UnitNullSprite;
    public UILinker Except;
    public RectTransform Content;
    public GameObject SquadPrefab;

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
    }
}
