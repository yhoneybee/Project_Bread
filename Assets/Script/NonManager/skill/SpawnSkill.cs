using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSkill : BaseSkill
{
    public override void Cast(Unit target)
    {
        base.Cast(target);
        for (int i = 0; i < spawnCount; i++)
        {
            int x = i * spacingX;
            Instantiate(this);
        }
    }
}