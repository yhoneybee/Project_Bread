using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance { get; private set; } = null;
    public static List<Unit> Select => GameManager.Instance.Decks[Instance.Index];

    private int index = 0;
    public int Index
    {
        get { return index; }
        set
        {
            index = value;
            DeckApply();
        }
    }

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

        for (int i = 0; i < GameManager.Instance.Decks.Count; ++i)
            if (GameManager.Instance.Decks[i].Count == 0)
                GameManager.Instance.Decks[i] = nulls;

        DeckApply();

        for (int i = 0; i < GameManager.Instance.Decks.Count; i++)
        {
            if (i >= LockStartIndex - 1)
                UIManager.Instance.TeamBtnLocks[i].Lock();
            else
                UIManager.Instance.TeamBtnLocks[i].UnLock();
        }
    }

    public void DeckApply()
    {
        for (int i = 0; i < MaxUnits; i++)
        {
            var view = UIManager.Instance.UnitViews[i];
            var unit = Select[i];
            view.Show = unit;
        }
    }
}