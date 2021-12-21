using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkillObj : ScriptableObject
{
    public abstract void Excute(Unit unit);

    public abstract void UnExcute(Unit unit);
}
