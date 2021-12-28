using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBuff : BaseSkill
{
    public float range;
    public UnitType buffTarget;

    protected override void Start()
    {
        base.Start();
    }

    public override void Cast()
    {
        base.Cast();
        goSkills.Add(Instantiate(originSkill, owner.transform.position, Quaternion.identity));
    }

    protected override void Update()
    {
        base.Update();
        if (duractionDone)
            goSkills.ForEach(x => Destroy(x.gameObject));
    }
}
