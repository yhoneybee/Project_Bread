using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedObj : MonoBehaviour
{
    public Skill context;
    public float damage;
    public bool useDamageValue;

    private void Start()
    {
        Destroy(gameObject, context.spawn.duraction);
    }

    private void Update()
    {
        transform.Translate(Vector3.right * context.spawn.speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var unit = collision.GetComponent<Unit>();
        if (!unit) return;

        if (unit.UnitType == UnitType.FRIEND) return;

        if (useDamageValue)
        {
            unit.OnHit(context.owner, damage);
            IngameManager.Instance.DamageText((int)damage, unit.transform.position);
        }
        else
        {
            context.owner.OnAttack(unit);
        }

        Destroy(gameObject);
    }
}
