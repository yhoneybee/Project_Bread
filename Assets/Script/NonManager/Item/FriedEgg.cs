using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriedEgg : Item
{
    bool active = false;

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
            MonoOwner.StartCoroutine(E1SecAfterActiveFor7Sec());
        }
    }

    public override void OnHit(Unit take, ref float damage)
    {
    }

    public override void UnEquip()
    {
    }

    IEnumerator E1SecAfterActiveFor7Sec()
    {
        yield return new WaitForSeconds(1);
        float add = Owner.Stat.LS / 100 * 6 * Owner.Info.Level;
        Owner.Stat.LS += add;
        yield return new WaitForSeconds(2);
        Owner.Stat.LS -= add;
        yield return new WaitForSeconds(7);
        active = false;
    }
}
