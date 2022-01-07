using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AMoveOption : AOption
{
    [Header("몇초 동안 돌진을 하는지")]
    public float duraction = 5;
    [Header("얼마나 빠르게 돌진하는지")]
    public float speed = 1;
    [Header("유닛이 돌진중에 다른 유닛을 통과하는지")]
    public bool isTrigger;
    [Header("돌진중에 넉백이 있는지")]
    public bool hasKnockback;
    [Header("넉백 힘")]
    public Vector2 knockbackForce;
    [Header("돌진중에 데미지가 있는지")]
    public bool hasDamage;
}
