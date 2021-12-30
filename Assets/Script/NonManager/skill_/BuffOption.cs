using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(fileName = "BuffOption", menuName = "Datas/Options/BuffOption")]
public class BuffOption : ScriptableObject
{
    [HideInInspector] public Skill context;
    [Header("AD MS AS HP MaxHP�� Buffȿ���� �����")]
    public Stat buff;
    public UnitType target;
    [Header("���� ���� �ð�")]
    public float duraction;
    [Header("���� �ش� ����")]
    public bool onlyMe;

    public IEnumerator EInvoke(Unit unit)
    {
        if (unit.UnitType != target) yield break;

        unit.Stat *= buff;

        yield return new WaitForSeconds(duraction);

        unit.Stat = UnitManager.Instance.Units.Find(x => unit.Info.Name == x.Info.Name).Stat;

        yield return null;
    }
}