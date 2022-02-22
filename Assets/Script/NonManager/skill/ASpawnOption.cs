using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ASpawnOption : AOption
{
    public GameObject origin;
    public int spawnCount;
    public float spawnDelay;
    public Vector2 cell;
    public Vector2 spacing;
    public Vector2 offset;
    [Header("근처 랜덤 소환 여부 (cell, spacing 무시)")]
    public bool isNear;
    public float range;
    [Header("한번에 소환 여부 (spawnDelay 무시)")]
    public bool isOnce;
    [Header("피격시 발동 여부")]
    public bool isOnHit;
    [Header("소환되는 객체에 대한 값")]
    public bool isGuide;
    public float speed;
    public float duraction;
    public bool parentToSkillOwner;
}
