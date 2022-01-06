using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(fileName = "DamageOption", menuName = "Datas/Options/DamageOption")]
public class DamageOption : AOption
{
    public override Skill Context { get => context; set => context = value; }
    private Skill context;

    [SerializeField, Header("데미지 배율 (ex) 20%추가 -> 1.2)")]
    private float damageRatio = 1;
    [SerializeField, Header("한번에 데미지를 주는지에 대한 여부 (tick~ 무시)")]
    private bool isOnce;
    [SerializeField, Header("isOnce: false, 데미지를 나눠주는 지속시간")]
    private float duraction;
    [SerializeField, Header("데미지를 몇초마다 주는지에 대한 값")]
    private float tick;
    [SerializeField, Header("tick 데미지 배율 (ex) 20%추가 -> 1.2)")]
    private float tickDamageRatio = 1;
    [SerializeField, Header("절대 tick 데미지 0이 아닐경우 반영됨")]
    private float absTickDamage = 0;
    [SerializeField, Header("딜레이")]
    private float damageDelay = 0;

    public float TotalDamage => context.owner.Stat.AD * damageRatio;
    public float TickCount => duraction / tick;
    public float TickDamage => TotalDamage / TickCount;

    public override IEnumerator EInvoke(Unit unit)
    {
        var wait = new WaitForSeconds(tick);
        yield return new WaitForSeconds(damageDelay);
        for (int i = 0; !isOnce && i < TickCount; i++)
        {
            if (absTickDamage != 0) unit.Stat.HP -= absTickDamage;
            else unit.Stat.HP -= TickDamage * tickDamageRatio;
            IngameManager.Instance.DamageText(absTickDamage != 0 ? (int)absTickDamage : (int)(TickDamage * tickDamageRatio), unit.transform.position);
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