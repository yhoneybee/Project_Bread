using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    public Unit owner;
    public Sprite sprSkill;
    public GameObject originSkill;
    public GameObject goSkill;
    public float coolDown;
    public float coolTime;
    public bool CoolDone => coolDown == 0;
    public int count;
    public int countDown;
    public float totalDamage;
    [Header("공격력에 곱해질 예정")]
    public float damageRatio;
    public float duraction;
    public float duractionDown;
    public bool duractionDone => duractionDown == 0;

    protected virtual void Update()
    {
        if (coolDown > 0) coolDown -= Time.deltaTime;
        else coolDown = 0;
        if (duractionDown > 0) duraction -= Time.deltaTime;
        else duractionDown = 0;
    }

    public virtual void Cast()
    {
        if (!CoolDone) return;

        coolDown = coolTime;
        duractionDown = duraction;
        owner.AnimState = AnimState.SKILL;
        totalDamage = owner.Stat.AD * damageRatio;
        countDown = count;
    }

    public void ResetCount()
    {
        countDown = 0;
    }
}
