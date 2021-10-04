using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }

    public override void OnAnimChanged()
    {
    }
    public override void OnEndFrameAnim()
    {
    }
    public override void OnHit(Unit taker, float damage)
    {
        base.OnHit(taker, damage);
    }
    public override void OnAttack(Unit taken)
    {
        base.OnAttack(taken);
    }
}
