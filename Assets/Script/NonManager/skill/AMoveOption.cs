using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AMoveOption : AOption
{
    public abstract bool HasKnockback { get; set; }
    public abstract Vector2 KnockbackForce { get; set; }
    public abstract bool HasDamage { get; set; }
}
