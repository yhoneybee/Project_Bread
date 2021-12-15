using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    public BaseSkill goSkill;
    public Sprite skillIcon;
    public float coolTime;
    public float curCoolTime;
    public bool IsCoolDown => curCoolTime == 0;

    public virtual void Update()
    {
        curCoolTime -= Time.deltaTime;
        if (curCoolTime <= 0) curCoolTime = 0;
    }

    public abstract void Excute(Collider2D col2D);

    public virtual void Cast(Unit target)
    {
        curCoolTime = coolTime;
    }

    private void OnTriggerEnter2D(Collider2D col2D)
    {
        Excute(col2D);
    }
}
