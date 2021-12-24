using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    public Unit owner;
    public Sprite sprSkill;
    public GameObject originSkill;
    public GameObject goSkill;
    public RuntimeAnimatorController controller;
    public float coolDown;
    public float coolTime;
    public bool CoolDone => coolDown == 0;
    public int count;
    public int countDown;
    public float totalDamage;
    [Header("공격력에 곱해질 예정")]
    public float damageRatio;
    public float duractionDown;
    public bool duractionDone => duractionDown == 0;

    protected float duractionObj;
    protected float tick;

    private float duraction;

    protected virtual void Start()
    {
        var find = UnitManager.Instance.skillInfos.Find(x => x.name == owner.Info.Name);
        duraction = find.duraction;
        duractionObj = find.duractionObj;
        tick = find.tick;
    }

    protected virtual void Update()
    {
        if (coolDown > 0) coolDown -= Time.deltaTime;
        else coolDown = 0;
        if (duractionDown > 0) duractionDown -= Time.deltaTime;
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
