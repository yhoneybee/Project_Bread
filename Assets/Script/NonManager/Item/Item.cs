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
            if (owner != null && owner.Info.Name != value.Info.Name)
            {
                UnEquip();
                owner.Items.Remove(this);
            }
            owner = value;
        }
    }
    public Sprite Icon;
    public Stat Stat;
    public string Name;
    [Multiline(5)]
    public string Desc;

    public abstract void Ingame();
    public abstract void Equip();
    public abstract void UnEquip();
    public abstract void OnAttack(Unit taken);
    public abstract void OnHit(Unit take, ref float damage);
}
