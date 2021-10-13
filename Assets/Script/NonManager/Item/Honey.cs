using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Honey : Item
{
    int stack;

    public Honey(Unit owner) : base(owner)
    {
    }

    int Stack
    {
        get { return stack; }
        set { stack = Mathf.Min(value, 8); }
    }

    public override void Equip()
    {
    }

    public override void Ingame()
    {
        Stack = 0;
        MonoOwner = Owner.GetComponent<MonoBehaviour>();
    }

    public override void OnAttack(Unit taken)
    {
        MonoOwner.CancelInvoke(nameof(ResetStack));
        ++Stack;
        if (Owner.Stat.AR <= 1.5f) ++Stack;
        taken.OnHit(Owner, Stack);
        MonoOwner.Invoke(nameof(ResetStack), 5);
    }

    public override void OnHit(Unit take, ref float damage)
    {
    }

    public void ResetStack() => Stack = 0;

    public override void UnEquip()
    {
    }
}
