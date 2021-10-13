using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawberryJam : Item
{
    public StrawberryJam(Unit owner) : base(owner)
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
        switch (Owner.Stat.AR)
        {
            case float f when f <= 1.5f:
                taken.OnHit(Owner, taken.Stat.MaxHP / 100 * 10);
                break;
            case float f when f > 1.5f:
                taken.OnHit(Owner, taken.Stat.MaxHP / 100 * 6);
                break;
        }
    }

    public override void OnHit(Unit take, ref float damage)
    {
    }

    public override void UnEquip()
    {
    }
}
