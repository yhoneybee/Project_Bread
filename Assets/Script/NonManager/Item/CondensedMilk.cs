using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CondensedMilk : Item
{
    bool active = false;
    Coroutine CDotDealing;

    public CondensedMilk(Unit owner) : base(owner)
    {
    }

    public override void Equip()
    {
    }

    public override void Ingame()
    {
        active = false;
        MonoOwner = Owner.GetComponent<MonoBehaviour>();
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
            if (CDotDealing != null) MonoOwner.StopCoroutine(CDotDealing);
            CDotDealing = MonoOwner.StartCoroutine(EDotDealing(Owner, taken));
        }
    }

    public override void OnHit(Unit take, ref float damage)
    {
    }

    public override void UnEquip()
    {
    }

    IEnumerator EDotDealing(Unit taker, Unit taken)
    {
        var wait = new WaitForSeconds(1);

        for (int i = 0; i < 3; i++)
        {
            taken.OnHit(taker, taker.Stat.AD / 2);
            yield return wait;
        }
    }
}
