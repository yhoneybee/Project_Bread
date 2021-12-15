using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    public GameObject originSkill;
    public GameObject goSkill;
    public Sprite skillIcon;
    public float coolTime;
    public float curCoolTime;
    public bool IsCoolDown => curCoolTime == 0;

    public virtual void Update()
    {
        curCoolTime -= Time.deltaTime;
        if (curCoolTime <= 0) curCoolTime = 0;
    }

    public virtual void Cast(Unit target)
    {
        curCoolTime = coolTime;
    }
}
