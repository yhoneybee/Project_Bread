using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(fileName = "BuffOption", menuName = "Datas/Options/BuffOption")]
public class BuffOption : ABuffOption
{
    public override IEnumerator EInvoke(Unit unit)
    {
        if (unit.UnitType != target) yield break;

        unit.Stat *= buff;

        yield return new WaitForSeconds(duraction);

        float hp = unit.Stat.HP;

        var stat = UnitManager.Instance.Units.Find(x => unit.Info.Name == x.Info.Name).Stat;
        unit.Stat = stat;
        unit.Stat.HP = hp;

        yield return null;
    }
}