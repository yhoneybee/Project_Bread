using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nutella : Item
{
    bool active = false;
    bool time_out = false;
    int count = 0;
    public override void Equip()
    {
    }

    public override void Ingame()
    {
        MonoOwner = Owner.GetComponent<MonoBehaviour>();
    }

    public override void OnAttack(Unit taken)
    {
        if (!active)
        {
            active = true;
            count = 3;
        }
        if (count > 0)
        {
            MonoOwner.CancelInvoke(nameof(ResetCount));
            --count;
            MonoOwner.Invoke(nameof(ResetCount), 2.5f);
        }
        if (!time_out && active && count == 0) taken.OnHit(Owner, Owner.Stat.AD / 100 * 40 + Owner.Info.Level * 10);
    }

    public override void OnHit(Unit take, ref float damage)
    {
    }

    public override void UnEquip()
    {
    }

    void ResetCount()
    {
        count = 0;
        time_out = true;
    }
}
