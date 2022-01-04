using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(fileName = "SpawnOption", menuName = "Datas/Options/SpawnOption")]
public class SpawnOption : ASpawnOption
{
    public override Skill Context { get => context; set => context = value; }
    private Skill context;

    public override bool IsGuide { get => isGuide; set => isGuide = value; }
    public override float Speed { get => speed; set => speed = value; }
    public override float Duraction { get => duraction; set => duraction = value; }

    [SerializeField] private GameObject origin;
    [SerializeField] private int spawnCount;
    [SerializeField] private int spawnDelay;
    [SerializeField] private Vector2 cell;
    [SerializeField] private Vector2 spacing;
    [SerializeField] private Vector2 offset;
    [SerializeField, Header("근처 랜덤 소환 여부 (cell, spacing 무시)")] 
    private bool isNear;
    [SerializeField] private float range;
    [SerializeField, Header("한번에 소환 여부 (spawnDelay 무시)")]
    private bool isOnce;
    [Header("소환되는 객체에 대한 값")]
    [SerializeField] private bool isGuide;
    [SerializeField] private float speed;
    [SerializeField] private float duraction;
    
    public override IEnumerator EInvoke(Unit unit)
    {
        var wait = new WaitForSeconds(spawnDelay);

        for (int i = 0; origin && i < spawnCount; i++)
        {
            var obj = Instantiate(origin);
            var spawned = obj.AddComponent<SpawnedObj>();
            spawned.context = context;

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