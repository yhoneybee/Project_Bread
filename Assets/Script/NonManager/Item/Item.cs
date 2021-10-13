using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public Item(Unit owner) => Owner = owner;

    Unit owner;
    public Unit Owner
    {
        get {  return owner; }
        set 
        {
            bool un_equip = false;

            if (!value) un_equip = true;
            else if (owner && owner.Info.Name != value.Info.Name) un_equip = true;

            if (un_equip)
            {
                UnEquip();
                owner.Items.Remove(this);
            }

            owner = value;
        }
    }
    public MonoBehaviour MonoOwner;
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
