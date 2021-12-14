using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSkill : BaseSkill
{
    public int shotCount;
    public override void Cast(Unit target)
    {
        for (int i = 0; i < shotCount; i++)
        {
            Instantiate(originSkill);
        }
    }
}
