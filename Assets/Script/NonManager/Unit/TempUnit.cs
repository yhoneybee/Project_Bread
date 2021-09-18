using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempUnit : Unit
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
        throw new System.NotImplementedException();
    }
    public override void OnBeginFrameAnim()
    {
        throw new System.NotImplementedException();
    }
    public override void OnEndFrameAnim()
    {
        throw new System.NotImplementedException();
    }
    public override void OnHit(Unit taker, float damage)
    {
        throw new System.NotImplementedException();
    }
}
