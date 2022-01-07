using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AMoveOption : AOption
{
    [Header("���� ���� ������ �ϴ���")]
    public float duraction = 5;
    [Header("�󸶳� ������ �����ϴ���")]
    public float speed = 1;
    [Header("������ �����߿� �ٸ� ������ ����ϴ���")]
    public bool isTrigger;
    [Header("�����߿� �˹��� �ִ���")]
    public bool hasKnockback;
    [Header("�˹� ��")]
    public Vector2 knockbackForce;
    [Header("�����߿� �������� �ִ���")]
    public bool hasDamage;
}
