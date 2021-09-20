using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } = null;

    public static UnitView SelectSlot;

    public Sprite TeamBtnLock;

    private void Awake()
    {
        Instance = this;
    }
}
