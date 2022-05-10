using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum eSKILL_TYPE
{
    Buff,
    Damage,
    Move,
    Spawn,
}

public class Skill : MonoBehaviour
{
    [HideInInspector] public Unit owner;
    [TextArea(3, 5)]
    public string strSkillInfo;
    public eSKILL_TYPE skillType;
    public Sprite sprSkill;
    public ASpawnOption spawn;
    public AOption damage;
    public ABuffOption buff;
    public AMoveOption move;
    public CoolOption cool;

    private void Start()
    {
        owner = GetComponent<Unit>();

        if (spawn != null) spawn.context = this;
        if (damage != null) damage.context = this;
        if (buff != null) buff.context = this;
        if (move != null) move.context = this;
        if (cool) cool.context = this;
    }

    public void Cast()
    {
        owner.AnimState = AnimState.SKILL;

        SoundManager.Instance.Play(owner.Info.skillSound, SoundType.EFFECT);

        if (!cool && !cool.CoolDone) return;

        cool.Initialization();
        if (buff != null && buff.isInstant) StartCoroutine(buff.EInvoke(owner));
        if (spawn != null) StartCoroutine(spawn.EInvoke(null));
        if (move != null) StartCoroutine(move.EInvoke(null));
    }

    private void Update()
    {
        if (!cool) return;

        cool.Update();
    }
}
