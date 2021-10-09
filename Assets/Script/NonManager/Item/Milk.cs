using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milk : Item
{
    public override void Equip()
    {
    }

    public override void OnAttack(Unit taken)
    {
        if (Owner.Stat.HP <= Owner.Stat.MaxHP / 2)
        {
            taken.OnHit(Owner, Owner.Stat.AD / 100 * (Owner.Stat.MaxHP - Owner.Stat.HP) * 0.1f);
        }
    }

    public override void OnHit(Unit take, ref float damage)
    {
    }

    public override void UnEquip()
    {
    }
}
