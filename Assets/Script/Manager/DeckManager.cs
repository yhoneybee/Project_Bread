using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance { get; private set; } = null;
    public static List<Unit> Select => Instance.Decks[Index];
    public List<List<Unit>> Decks { get; private set; } = new List<List<Unit>>();

    public static Unit SelectUnit;

    public static int Index = 0;
    public readonly int MaxUnits = 7;

    private void Awake()
    {
        Instance = this;
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