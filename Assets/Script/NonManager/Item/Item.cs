using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] Unit owner;
    public Unit Owner
    {
        get { return owner; }
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
    [TextArea(3,5)]
    public string Desc;
    public bool gotten;

    public abstract void Ingame();
    public abstract void Equip();
    public abstract void UnEquip();
    public abstract void OnAttack(Unit taken);
    public abstract void OnHit(Unit take, ref float damage);
}
