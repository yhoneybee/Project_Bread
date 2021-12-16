using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportanceTiming : BaseSkill
{
    public override void Cast(Unit target)
    {
        base.Cast(target);
        goSkill = Instantiate(originSkill, target.transform.position, Quaternion.identity);
        goSkill.GetComponent<CircleCollider2D>().radius = radius;
        Destroy(goSkill.gameObject, durationTime);
    }
}
