using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenGarden : BaseSkill
{
    public override void Cast(Unit target)
    {
        base.Cast(target);
        for (int i = 0; i < spawnCount; i++)
        {
            int x = i * spacingX;
            //Instantiate(originSkill, new Vector3(target.transform.position.x + x,), Quaternion.identity);
        }
    }

}
