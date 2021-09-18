using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public Unit Owner;
    public Stat Stat;

    protected virtual void Start()
    {
        
    }
    protected virtual void Update()
    {
        
    }

    public abstract void OnAttack(Unit taken);
    public abstract void OnHit(Unit taker);
}
