using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butter : Item
{
    bool active = false;

    public override void Equip()
    {
    }

    public override void Ingame()
    {
        active = false;
    }

    public override void OnAttack(Unit taken)
    {
    }

    public override void OnHit(Unit take, ref float damage)
    {
        if (!active && Owner.Stat.HP - damage <= 0)
        {
            active = true;
            damage = 0;
            Owner.Invincibility = true;
        }
    }

    public override void UnEquip()
    {
    }
}
