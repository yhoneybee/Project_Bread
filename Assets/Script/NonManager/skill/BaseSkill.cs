using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    public GameObject originSkill;
    public GameObject goSkill;
    public Sprite skillIcon;
    public float coolTime;

    public abstract void Cast(Unit target);
}
