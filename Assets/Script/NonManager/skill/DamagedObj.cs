using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedObj : MonoBehaviour
{
    public Skill context;
    public float damage;

    private void Start()
    {
        Destroy(gameObject, context.spawn.Duraction);
    }

    private void Update()
    {
        transform.Translate(Vector3.right * context.spawn.Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var unit = collision.GetComponent<Unit>();
        if (!unit) return;

        unit.Stat.HP -= damage;
        IngameManager.Instance.DamageText((int)damage, unit.transform.position);
    }
}
