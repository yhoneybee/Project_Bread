using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    Unit owner;
    public Unit Owner
    {
        get {  return owner; }
        set 
        {
            if (owner != null) owner.Items.Remove(this);
            owner = value;
        }
    }
    public Sprite Icon;
    public Stat Stat;
    public string Name;
    public string Desc;

    public abstract void OnAttack(Unit taker, Unit taken);
    public abstract void OnHit(Unit take, ref float damage);
}
