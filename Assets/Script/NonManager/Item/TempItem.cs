using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempItem : Item
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }

    public override void OnAttack(Unit taken)
    {
    }
    public override void OnHit(Unit taken)
    {
    }
}
