using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportanceTimingObj : SkillObj
{
    public override void Excute(Collider2D col2D)
    {
        base.Excute(col2D);
        var unit = col2D.GetComponent<Unit>();
        if (unit && unit.UnitType == UnitType.UNFRIEND)
        {
            unit.deltaSpeed = 0;
        }
    }

    public override void UnExcute(Collider2D col2D)
    {
        base.UnExcute(col2D);
        var unit = col2D.GetComponent<Unit>();
        if (unit && unit.UnitType == UnitType.UNFRIEND && unit.deltaSpeed == 0)
        {
            unit.deltaSpeed = 1;
        }
    }
}
