using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class SkillObj : MonoBehaviour
{
    public event Action<Unit> onEnter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var unit = collision.gameObject.GetComponent<Unit>();
        if (unit && unit.UnitType == UnitType.UNFRIEND) onEnter?.Invoke(unit);
    }
}
