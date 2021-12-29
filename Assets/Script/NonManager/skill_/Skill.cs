using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Skill : MonoBehaviour
{
    [HideInInspector] public Unit owner;
    public Sprite sprSkill;
    public SpawnOption spawn;
    public DamageOption damage;
    public BuffOption buff;
    public CoolOption cool;

    private void Start()
    {
        owner = GetComponent<Unit>();

        if (spawn) spawn.context = this;
        if (damage) damage.context = this;
        if (buff) buff.context = this;
        if (cool) cool.context = this;
    }

    public void Cast()
    {
        if (!cool && !cool.CoolDone) return;

        cool.Initialization();
        StartCoroutine(spawn.EInvoke());
    }

    private void Update()
    {
        if (!cool) return;

        cool.Update();
    }
}
