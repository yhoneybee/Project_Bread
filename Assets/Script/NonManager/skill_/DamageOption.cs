using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(fileName = "DamageOption", menuName = "Datas/Options/DamageOption")]
public class DamageOption : ScriptableObject
{
    [HideInInspector] public Skill context;
    [Header("������ ���� (ex) 20%�߰� -> 1.2)")]
    public float damageRatio;
    [Header("�ѹ��� �������� �ִ����� ���� ���� (tick~ ����)")]
    public bool isOnce;
    [Header("isOnce: false, �������� �����ִ� ���ӽð�")]
    public float duraction;
    [Header("�������� ���ʸ��� �ִ����� ���� ��")]
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