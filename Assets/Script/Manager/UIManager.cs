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
    public RectTransform Except;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (AllUnits != null)
        {
            var friends = UnitManager.Instance.Units.FindAll((o) => { return o.UnitType == UnitType.FRIEND; });
            for (int i = 0; i < AllUnits.Count; i++)
            {
                var view = AllUnits[i];
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
                var find = AllUnits.Find((o) => { return o.Show == select_unit; });
                if (find) find.gameObject.SetActive(false);
            }
        }
    }
}
