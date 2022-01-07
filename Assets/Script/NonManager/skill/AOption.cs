using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AOption : ScriptableObject
{
    public abstract IEnumerator EInvoke(Unit unit);
    [HideInInspector] public Skill context;
}