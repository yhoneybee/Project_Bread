using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bacon : Item
{
    public override void Equip()
    {
    }

    public override void OnAttack(Unit taken)
    {
        if (taken.Stat.HP <= taken.Stat.MaxHP / 100 * 40)
        {
            taken.OnHit(Owner, Owner.Stat.AD / 100 * 8);
        }
    }

    public override void OnHit(Unit take, ref float damage)
    {
    }

    public override void UnEquip()
    {
    }
}
