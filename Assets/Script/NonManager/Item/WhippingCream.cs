using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhippingCream : Item
{
    bool active = false;
    int count = 0;
    float add = 0;
    public override void Equip()
    {
    }

    public override void OnAttack(Unit taken)
    {
        if (Owner.WalkAble)
        {
            active = true;
        }
        if (active)
        {
            active = false;
            count = 3;
        }
        if (count == 3)
        {
            add = Owner.Stat.LS / 100 * 110;
            Owner.Stat.LS += add;
        }
        else if (count == 0)
            Owner.Stat.LS -= add;

        if (count > 0) --count;
    }

    public override void OnHit(Unit take, ref float damage)
    {
    }

    public override void UnEquip()
    {
    }
}
