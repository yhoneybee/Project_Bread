using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueberryJam : Item
{
    bool active = false;
    private void Start()
    {
        active = false;
    }
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
            Owner.Stat.HP += Owner.Stat.AD / 100 * 2 * Owner.Info.Level;
        }
    }

    public override void OnHit(Unit take, ref float damage)
    {
    }

    public override void UnEquip()
    {
    }
}
