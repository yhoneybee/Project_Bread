using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public Unit Owner;
    public Sprite Icon;
    public Stat Stat;
    public string Name;
    public string Desc;

    public abstract void OnAttack(Unit taken);
    public abstract void OnHit(Unit take, ref float damage);
}
