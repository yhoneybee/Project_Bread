using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(fileName = "CoolOption", menuName = "Datas/Options/CoolOption")]
public class CoolOption : ScriptableObject
{
    [HideInInspector] public Skill context;
    [SerializeField, Header("쿨타임")]
    private float coolTime;
    [SerializeField, Header("남은 쿨타임 (값 확인용)")]
    private float coolDown;
    public bool CoolDone => coolDown == 0 && duractionDone;

    [SerializeField, Header("스킬 지속 시간")]
    private float duraction;
    [SerializeField, Header("남은 스킬 지속 시간 (값 확인용)")]
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