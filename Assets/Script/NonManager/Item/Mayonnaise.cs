using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mayonnaise : Item
{
    float add = 0;

    public Mayonnaise(Unit owner) : base(owner)
    {
    }

    public override void Equip()
    {
    }

    public override void Ingame()
    {
        add = 0;
    }

    public override void OnAttack(Unit taken)
    {
    }

    public override void OnHit(Unit take, ref float damage)
    {
        if (add == 0 && Owner.Stat.HP <= Owner.Stat.MaxHP / 100 * 40)
        {
            add = Owner.Stat.LS / 100 * 10;
            Owner.Stat.LS += add;
        }
        else
        {
            Owner.Stat.HP -= add;
            add = 0;
        }
    }

    public override void UnEquip()
    {
    }
}
