using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSkill : BaseSkill
{
    public int shotCount;
    public override void Cast(Unit target)
    {
        base.Cast(target);
        for (int i = 0; i < shotCount; i++)
        {
            Instantiate(this);
        }
    }

    public override void Excute(Collider2D col2D)
    {
    }
}
