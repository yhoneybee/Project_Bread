using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class SpawnedObj : MonoBehaviour
{
    public SpriteRenderer sr;
    public Skill context;
    public Anim anim;
    public int delayOfFrameCount;
    public bool continueToDuraction;
    public bool shotable;
    public DamagedObj shotObj;
    public bool onDestroySpawnObj;
    public SpawnedObj destroySpawnObj;
    public bool isStay;

    private void Start()
    {
        Destroy(gameObject, context.spawn.duraction);
        StartCoroutine(EAnim());
        if (shotable) InvokeRepeating(nameof(Shot), 0, 0.5f);
    }

    private void Update()
    {
        if (context.spawn.isGuide)
        {

        }
        else if (!isStay)
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

    IEnumerator EAnim()
    {
        var wait = new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(delayOfFrameCount * 0.1f);

        var v2 = Vector2.zero;
        var box2D = GetComponent<BoxCollider2D>();

        for (int i = 0; i < anim.Skill.Count; i++)
        {
            sr.sprite = anim.Skill[i];

            v2 = sr.sprite.bounds.size;
            box2D.size = v2;
            box2D.offset = Vector2.zero;

            yield return wait;
        }

        if (!continueToDuraction)
            Destroy(gameObject);

        yield return null;
    }

    private void Shot()
    {
        var obj = Instantiate(shotObj, transform.position, Quaternion.identity);
        obj.context = context;
    }

    private void OnDestroy()
    {
        if (onDestroySpawnObj)
        {
            var obj = Instantiate(destroySpawnObj, transform.position, Quaternion.identity);
            obj.context = context;
        }
    }
}