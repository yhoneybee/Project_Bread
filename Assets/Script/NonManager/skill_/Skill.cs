using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[Serializable, CreateAssetMenu(fileName = "SpawnOption", menuName = "Datas/Options/SpawnOption")]
public class SpawnOption : ScriptableObject
{
    public Skill context;
    public GameObject origin;
    public int spawnCount;
    public int spawnDelay;
    public Vector2 cell;
    public Vector2 spacing;
    public Vector2 offset;
    [Header("근처 랜덤 소환 여부 (cell, spacing 무시)")]
    public bool isNear;
    public float range;
    [Header("한번에 소환 여부 (spawnDelay 무시)")]
    public bool isOnce;
    [Header("소환되는 객체에 대한 값")]
    public bool isGuide;
    public float speed;
    public float duraction;

    public IEnumerator EInvoke()
    {
        var wait = new WaitForSeconds(spawnDelay);

        for (int i = 0; origin && i < spawnCount; i++)
        {
            var obj = Instantiate(origin);
            obj.AddComponent<SpawnedObj>().context = context;

            if (isNear)
            {
                obj.transform.position = new Vector3(offset.x + UnityEngine.Random.Range(-range, range), offset.y + UnityEngine.Random.Range(-range, range));
            }
            else
            {
                obj.transform.position = new Vector3(offset.x + (cell.x + spacing.x) * i, offset.y + (cell.y + spacing.y) * i);
            }
            if (!isOnce) yield return wait;
        }

        yield return null;
    }
}
[Serializable, CreateAssetMenu(fileName = "DamageOption", menuName = "Datas/Options/DamageOption")]
public class DamageOption : ScriptableObject
{
    public Skill context;
    public float damage;
    public float damageRatio;
    public bool isOnce;
    public float duraction;
    public float tick;
    public float TotalDamage => damage * damageRatio;
    public float TickCount => duraction / tick;
    public float TickDamage => TotalDamage / TickCount;

    public IEnumerator EInvoke(Unit unit)
    {
        var wait = new WaitForSeconds(tick);
        for (int i = 0; !isOnce && i < TickCount; i++)
        {
            unit.Stat.HP -= TickDamage;
            IngameManager.Instance.StartCoroutine(IngameManager.Instance.DamageText((int)TickDamage, context.owner.transform.position));
            yield return wait;
        }

        if (isOnce)
        {
            unit.Stat.HP -= TotalDamage;
            IngameManager.Instance.StartCoroutine(IngameManager.Instance.DamageText((int)TotalDamage, context.owner.transform.position));
        }

        yield return null;
    }
}
[Serializable, CreateAssetMenu(fileName = "BuffOption", menuName = "Datas/Options/BuffOption")]
public class BuffOption : ScriptableObject
{
    public Skill context;
    public Stat buff;
    public UnitType target;
    public float duraction;

    public IEnumerator EInvoke(Unit unit)
    {
        if (unit.UnitType != target) yield break;

        unit.Stat *= buff;

        yield return new WaitForSeconds(duraction);

        unit.Stat = UnitManager.Instance.Units.Find(x => unit.Info.Name == x.Info.Name).Stat;

        yield return null;
    }
}
[Serializable, CreateAssetMenu(fileName = "CoolOption", menuName = "Datas/Options/CoolOption")]
public class CoolOption : ScriptableObject
{
    public Skill context;
    public float coolTime;
    public float coolDown;
    public bool CoolDone => coolDown == 0;

    public float duraction;
    public float duractionDown;
    public bool duractionDone => duractionDown == 0;

    public void Initialization() => coolDown = coolTime;
    public void Update()
    {
        if (coolDown > 0) coolDown -= Time.deltaTime;
        else coolDown = 0;
        if (duractionDown > 0) duractionDown -= Time.deltaTime;
        else duractionDown = 0;
    }
}

public class Skill : MonoBehaviour
{
    public Unit owner;
    public SpawnOption spawn;
    public DamageOption damage;
    public BuffOption buff;
    public CoolOption cool;

    private void Start()
    {
        owner = GetComponent<Unit>();

        if (spawn) spawn.context = this;
        if (damage) damage.context = this;
        if (buff) buff.context = this;
        if (cool) cool.context = this;
    }

    public void Cast()
    {
        if (!cool.CoolDone) return;

        StartCoroutine(spawn.EInvoke());
    }

    private void Update()
    {
        if (!cool) return;

        cool.Update();
    }
}
