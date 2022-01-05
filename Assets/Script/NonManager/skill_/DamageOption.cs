using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(fileName = "DamageOption", menuName = "Datas/Options/DamageOption")]
public class DamageOption : AOption
{
    public override Skill Context { get => context; set => context = value; }
    private Skill context;

    [SerializeField, Header("������ ���� (ex) 20%�߰� -> 1.2)")]
    private float damageRatio = 1;
    [SerializeField, Header("�ѹ��� �������� �ִ����� ���� ���� (tick~ ����)")]
    private bool isOnce;
    [SerializeField, Header("isOnce: false, �������� �����ִ� ���ӽð�")]
    private float duraction;
    [SerializeField, Header("�������� ���ʸ��� �ִ����� ���� ��")]
    private float tick;
    [SerializeField, Header("tick ������ ���� (ex) 20%�߰� -> 1.2)")]
    private float tickDamageRatio = 1;
    [SerializeField, Header("���� tick ������ 0�� �ƴҰ�� �ݿ���")]
    private float absTickDamage = 0;
    [SerializeField, Header("������")]
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