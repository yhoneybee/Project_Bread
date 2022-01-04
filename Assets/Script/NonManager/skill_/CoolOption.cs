using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(fileName = "CoolOption", menuName = "Datas/Options/CoolOption")]
public class CoolOption : ScriptableObject
{
    [HideInInspector] public Skill context;
    [Header("��Ÿ��")]
    public float coolTime;
    [Header("���� ��Ÿ�� (�� Ȯ�ο�)")]
    public float coolDown;
    public bool CoolDone => coolDown == 0 && duractionDone;

    [SerializeField, Header("��ų ���� �ð�")]
    private float duraction;
    [SerializeField, Header("���� ��ų ���� �ð� (�� Ȯ�ο�)")]
    private float duractionDown;
    public bool duractionDone => duractionDown == 0;

    public void Initialization()
    {
        coolDown = coolTime;
        duractionDown = duraction;
    }
    public void Update()
    {
        if (coolDown > 0) coolDown -= Time.deltaTime;
        else coolDown = 0;
        if (duractionDown > 0) duractionDown -= Time.deltaTime;
        else duractionDown = 0;
    }
}