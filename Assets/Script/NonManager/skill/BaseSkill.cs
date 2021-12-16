using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    public SkillObj goSkill;
    public Sprite skillIcon;
    [Header("## ��Ÿ�� : ")]
    public float coolTime;
    public float curCoolTime;
    [Header("## ���ӽð� : ")]
    public float durationTime;
    [Header("## ��ȯ ��ų ���� : ")]
    public SkillObj originSkill;
    public int spawnCount;
    public int spacingX;
    [Header("## �߻� ��ų ���� : ")]
    public int shotCount;
    [Header("## ���� ��ų ���� : ")]
    public float radius;
    public bool IsCoolDown => curCoolTime <= 0;

    public virtual void Cast(Unit target)
    {
        curCoolTime = coolTime;
    }
}
