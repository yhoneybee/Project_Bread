using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class SpawnedObj : MonoBehaviour
{
    public Skill context;

    private void Start()
    {
        Destroy(gameObject, context.spawn.duraction);
    }

    private void Update()
    {
        if (context.spawn.isGuide)
        {

        }
        else
        {
            transform.Translate(Vector3.right * context.spawn.speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var unit = collision.GetComponent<Unit>();
        if (!unit) return;

        if (context.damage != null && unit.UnitType == UnitType.UNFRIEND)
            unit.StartCoroutine(context.damage.EInvoke(unit));
        if (context.buff != null && !context.buff.isInstant)
        {
            if (context.buff.onlyMe && unit.Info.Name != context.owner.Info.Name) return;
            unit.StartCoroutine(context.buff.EInvoke(unit));
        }
    }
}