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
            for (int i = 0; i < AllUnits.Count; i++)
            {
                var view = AllUnits[i];
                var show = UnitManager.Instance.Units[i];
                view.Show = show;
            }

            var select_unit = DeckManager.Select[GameManager.SelectSlotIdx];

            if (select_unit && Except)
            {
                Except.gameObject.SetActive(true);
                var find = AllUnits.Find((o) => { return o.Show == select_unit; });
                if (find) find.gameObject.SetActive(false);
            }
        }
    }
}
