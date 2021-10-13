using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheddarCheese : Item
{
    public CheddarCheese(Unit owner) : base(owner)
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
            case float f when f >= 2 && f < 2.5f: taken.OnHit(Owner, Owner.Stat.AD / 100 * 5); break;
            case float f when f >= 2.5f && f < 3: taken.OnHit(Owner, Owner.Stat.AD / 100 * 7.5f); break;
            case float f when f >= 3: taken.OnHit(Owner, Owner.Stat.AD / 100 * 10); break;
        }
    }

    public override void OnHit(Unit take, ref float damage)
    {
    }

    public override void UnEquip()
    {
    }
}
