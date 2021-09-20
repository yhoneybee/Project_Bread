using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } = null;

    public int Coin = 0;
    public int Jem = 0;
    public int Stemina = 0;
    public int MaxStemina = 0;

    private void Awake()
    {
        Instance = this;
    }
}
