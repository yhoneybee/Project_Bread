using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CondensedMilk : Item
{
    bool active = false;
    Coroutine CDotDealing;
    public override void OnAttack(Unit taker, Unit taken)
    {
        if (taker.WalkAble)
        {
            active = true;
        }
        if (active)
        {
            active = false;
            if (CDotDealing != null) StopCoroutine(CDotDealing);
            CDotDealing = StartCoroutine(EDotDealing(taker, taken));
        }
    }

    public override void OnHit(Unit take, ref float damage)
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
