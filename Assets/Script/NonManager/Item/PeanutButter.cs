using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeanutButter : Item
{
    public PeanutButter(Unit owner) : base(owner)
    {
    }

    public override void Equip()
    {
    }

    public override void Ingame()
    {
    }

    public override void OnAttack(Unit taken)
    {
        if (taken.Stat.HP <= taken.Stat.MaxHP / 100 * 5)
            if (!taken.Invincibility) taken.Stat.HP = 0;
    }

    public override void OnHit(Unit take, ref float damage)
    {
    }

    public override void UnEquip()
    {
    }
}
