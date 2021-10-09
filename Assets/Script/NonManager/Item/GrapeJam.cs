using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeJam : Item
{
    float add = 0;

    public override void Equip()
    {
        add = 0.6f * Owner.Info.Level;
        Owner.Stat.LS += add;
    }

    public override void OnAttack(Unit taken)
    {
    }

    public override void OnHit(Unit take, ref float damage)
    {
    }

    public override void UnEquip()
    {
        Owner.Stat.LS -= add;
    }
}
