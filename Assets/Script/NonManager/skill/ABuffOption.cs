using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ABuffOption : AOption
{
    [SerializeField, Header("AD MS AS HP MaxHP만 Buff효과로 적용됨")]
    public Stat buff;
    [SerializeField, Header("타겟 설정")]
    public UnitType target;
    [SerializeField, Header("버프 지속 시간")]
    public float duraction;
    [SerializeField, Header("본인 해당 여부")]
    public bool onlyMe;
    [SerializeField, Header("즉발 여부")]
    public bool isInstant;
}
