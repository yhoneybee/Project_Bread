using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(fileName = "BuffOption", menuName = "Datas/Options/BuffOption")]
public class BuffOption : ABuffOption
{
    public override Skill Context { get => context; set => context = value; }
    private Skill context;

    public override bool OnlyMe { get => onlyMe; set => onlyMe = value; }

    [SerializeField, Header("AD MS AS HP MaxHP�� Buffȿ���� �����")] 
    private Stat buff;
    [SerializeField, Header("Ÿ�� ����")]
    private UnitType target;
    [SerializeField, Header("���� ���� �ð�")]
    private float duraction;
    [SerializeField, Header("���� �ش� ����")]
    private bool onlyMe;

    public override IEnumerator EInvoke(Unit unit)
    {
        if (unit.UnitType != target) yield break;

        unit.Stat *= buff;

        yield return new WaitForSeconds(duraction);

        float hp = unit.Stat.HP;

        unit.Stat = UnitManager.Instance.Units.Find(x => unit.Info.Name == x.Info.Name).Stat;
        unit.Stat.HP = hp;

        yield return null;
    }
}