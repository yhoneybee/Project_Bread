using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } = null;

    public List<List<Unit>> Decks { get; private set; } = new List<List<Unit>>()
    {
        new List<Unit>(), new List<Unit>(), new List<Unit>(),
        new List<Unit>(), new List<Unit>(), new List<Unit>(),
        new List<Unit>(), new List<Unit>(), new List<Unit>(),
    };

    /*public static UnitView SelectSlot;*/
    public static int SelectSlotIdx;

    public int Coin = 0;
    public int Jem = 0;
    public int Stemina = 0;
    public int MaxStemina = 0;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
