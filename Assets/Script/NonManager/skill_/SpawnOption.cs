using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(fileName = "SpawnOption", menuName = "Datas/Options/SpawnOption")]
public class SpawnOption : ScriptableObject
{
    [HideInInspector] public Skill context;
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
                obj.transform.position = new Vector3(context.owner.transform.position.x + offset.x + (cell.x + spacing.x) * i, context.owner.transform.position.y + offset.y + (cell.y + spacing.y) * i, -1);
            }
            if (!isOnce) yield return wait;
        }

        yield return null;
    }
}