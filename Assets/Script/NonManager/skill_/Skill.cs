using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Skill : MonoBehaviour
{
    [HideInInspector] public Unit owner;
    public Sprite sprSkill;
    public ASpawnOption spawn;
    public AOption damage;
    public ABuffOption buff;
    public CoolOption cool;

    private void Start()
    {
        owner = GetComponent<Unit>();

        if (spawn != null) spawn.Context = this;
        if (damage != null) damage.Context = this;
        if (buff != null) buff.Context = this;
        if (cool) cool.context = this;
    }

    public void Cast()
    {
        if (!cool && !cool.CoolDone) return;

        cool.Initialization();
        if (buff != null && buff.IsInstant) StartCoroutine(buff.EInvoke(null));
        if (spawn != null) StartCoroutine(spawn.EInvoke(null));
    }

    private void Update()
    {
        if (!cool) return;

        cool.Update();
    }
}
