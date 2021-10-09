using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueberryJam : Item
{
    bool active = false;
    public override void OnAttack(Unit taker, Unit taken)
    {
        if (taker.WalkAble)
        {
            active = true;
        }
        if (active)
        {
            active = false;
            taker.Stat.HP += taker.Stat.AD / (2 * taker.Info.Level);
        }
    }

    public override void OnHit(Unit take, ref float damage)
    {
    }
}
