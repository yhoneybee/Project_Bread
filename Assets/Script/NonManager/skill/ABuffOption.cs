using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ABuffOption : AOption
{
    [SerializeField, Header("AD MS AS HP MaxHP�� Buffȿ���� �����")]
    public Stat buff;
    [SerializeField, Header("Ÿ�� ����")]
    public UnitType target;
    [SerializeField, Header("���� ���� �ð�")]
    public float duraction;
    [SerializeField, Header("���� �ش� ����")]
    public bool onlyMe;
    [SerializeField, Header("��� ����")]
    public bool isInstant;
}
