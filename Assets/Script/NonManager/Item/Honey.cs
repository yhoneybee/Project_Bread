using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Honey : Item
{
    int stack;
    int Stack
    {
        get { return stack; }
        set { stack = Mathf.Min(value, 8); }
    }
    public override void OnAttack(Unit taker, Unit taken)
    {
        CancelInvoke(nameof(ResetStack));
        ++Stack;
        if (taker.Stat.AR <= 1.5f) ++Stack;
        taken.OnHit(taker, Stack);
        Invoke(nameof(ResetStack), 5);
    }

    public override void OnHit(Unit take, ref float damage)
    {
    }

    public void ResetStack() => Stack = 0;
}
