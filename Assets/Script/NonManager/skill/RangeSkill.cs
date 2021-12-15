using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSkill : BaseSkill
{
    public float durationTime;
    
    public override void Cast(Unit target)
    {
        base.Cast(target);
        goSkill = Instantiate(originSkill);
        Destroy(goSkill, durationTime);
    }
}
