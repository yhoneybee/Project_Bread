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
    [Header("��ó ���� ��ȯ ���� (cell, spacing ����)")]
    public bool isNear;
    public float range;
    [Header("�ѹ��� ��ȯ ���� (spawnDelay ����)")]
    public bool isOnce;
    [Header("�ǰݽ� �ߵ� ����")]
    public bool isOnHit;
    [Header("��ȯ�Ǵ� ��ü�� ���� ��")]
    public bool isGuide;
    public float speed;
    public float duraction;
    public bool parentToSkillOwner;
}
