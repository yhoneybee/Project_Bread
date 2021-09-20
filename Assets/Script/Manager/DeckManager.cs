using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance { get; private set; } = null;
    public static List<Unit> Select => Instance.Decks[Index];
    public List<List<Unit>> Decks { get; private set; } = new List<List<Unit>>()
    {
        new List<Unit>(), new List<Unit>(), new List<Unit>(),
        new List<Unit>(), new List<Unit>(), new List<Unit>(),
        new List<Unit>(), new List<Unit>(), new List<Unit>(),
    };

    public static Unit SelectUnit;

    public List<TeamBtnLock> TeamBtnLocks = new List<TeamBtnLock>();

    public static int Index = 0;
    public readonly int MaxUnits = 7;
    public int LockStartIndex = 5;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        for (int i = 0; i < Decks.Count; i++)
        {
            if (i >= LockStartIndex - 1)
                TeamBtnLocks[i].Lock();
            else
                TeamBtnLocks[i].UnLock();
        }
    }

    public static void ChangeUnit(int index)
    {
        var find = Select.Find((o) => { return o.Info.Name == SelectUnit.Info.Name; });
        int from = 0;
        if (find)
        {
            from = Select.IndexOf(find);
            var temp = Select[from];
            Select[from] = Select[index];
            Select[index] = temp;
        }
        Select[index] = SelectUnit;
    }
}