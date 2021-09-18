using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public Unit Owner;
    public Stat Stat;

    public abstract void OnAttack(Unit taken);
    public abstract void OnHit(Unit take);
}
