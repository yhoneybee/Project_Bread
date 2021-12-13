using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    public Unit owner;
    public Sprite skillIcon;
    public float coolTime;

    public abstract void Cast();
}
