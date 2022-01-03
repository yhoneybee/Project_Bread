using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(fileName = "DamageOption", menuName = "Datas/Options/DamageOption")]
public class DamageOption : ScriptableObject
{
    [HideInInspector] public Skill context;
    [Header("데미지 배율 (ex) 20%추가 -> 1.2)")]
    public float damageRatio;
    [Header("한번에 데미지를 주는지에 대한 여부 (tick~ 무시)")]
    public bool isOnce;
    [Header("isOnce: false, 데미지를 나눠주는 지속시간")]
    public float duraction;
    [Header("데미지를 몇초마다 주는지에 대한 값")]
    public float tick;
    public float TotalDamage => context.owner.Stat.AD * damageRatio;
    public float TickCount => duraction / tick;
    public float TickDamage => TotalDamage / TickCount;

    public IEnumerator EInvoke(Unit unit)
    {
        var wait = new WaitForSeconds(tick);
        for (int i = 0; !isOnce && i < TickCount; i++)
        {
            unit.Stat.HP -= TickDamage;
            IngameManager.Instance.DamageText((int)TickDamage, unit.transform.position);
            yield return wait;
        }

        if (isOnce)
        {
            unit.Stat.HP -= TotalDamage;
            IngameManager.Instance.DamageText((int)TotalDamage, unit.transform.position);
        }

        yield return null;
    }
}