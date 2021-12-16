using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    public SkillObj goSkill;
    public Sprite skillIcon;
    [Header("## 쿨타임 : ")]
    public float coolTime;
    public float curCoolTime;
    [Header("## 지속시간 : ")]
    public float durationTime;
    [Header("## 소환 스킬 관련 : ")]
    public SkillObj originSkill;
    public int spawnCount;
    public int spacingX;
    [Header("## 발사 스킬 관련 : ")]
    public int shotCount;
    [Header("## 범위 스킬 관련 : ")]
    public float radius;
    public bool IsCoolDown => curCoolTime <= 0;

    public virtual void Cast(Unit target)
    {
        curCoolTime = coolTime;
    }
}
