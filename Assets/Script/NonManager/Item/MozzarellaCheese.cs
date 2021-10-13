using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MozzarellaCheese : Item
{
    float def = 200;

    public MozzarellaCheese(Unit owner) : base(owner)
    {
    }

    public override void Equip()
    {
    }

    public override void Ingame()
    {
        def = 200;
    }

    public override void OnAttack(Unit taken)
    {
    }

    public override void OnHit(Unit take, ref float damage)
    {
        if (def >= damage)
        {
            def -= damage;
            damage = 0;
        }
    }

    public override void UnEquip()
    {
    }
}
