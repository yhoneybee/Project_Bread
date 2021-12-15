using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance { get; private set; } = null;
    public static List<Unit> Select => GameManager.Select;

    /*public static Unit SelectUnit;*/

    public readonly int MaxUnits = 7;
    public int LockStartIndex = 5;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        List<Unit> nulls = new List<Unit>() { null, null, null, null, null, null, null, };

        if (SaveManager.Instance.IsFile("DeckSlot"))
            LockStartIndex = SaveManager.Load<int>("DeckSlot").FirstOrDefault();
        if (LockStartIndex == 0) LockStartIndex = 5;

        DeckApply();
    }

    public void DeckApply()
    {
        for (int i = 0; i < GameManager.Instance.Decks.Count; i++)
        {
            if (i >= LockStartIndex - 1)
                UIManager.Instance.TeamBtnLocks[i].Lock();
            else
                UIManager.Instance.TeamBtnLocks[i].UnLock();
        }

        for (int i = 0; i < MaxUnits; i++)
        {
            UIManager.Instance.UnitViews[i].Show = Select[i];
        }

        //if (ButtonActions.Instance.CheckReEntering("E - 01 DeckView"))
        //{
        //    foreach (var view in UIManager.Instance.UnitViews)
        //    {
        //        if (view && view.Show)
        //            view.UILinker.Icon.GetComponent<RectTransform>().sizeDelta /= view.Show.Info.DValue;
        //    }
        //}
    }

    public void AddSlot()
    {
        LockStartIndex++;
        DeckApply();
        GameManager.Instance.onAutoSave += () =>
        {
            SaveManager.Save(new List<int>() { LockStartIndex }, "DeckSlot");
        };
    }
}