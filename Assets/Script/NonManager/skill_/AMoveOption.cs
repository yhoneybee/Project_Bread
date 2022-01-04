using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMoveOption : AOption
{
    public override Skill Context { get => context; set => context = value; }
    [SerializeField] private Skill context;

    public override IEnumerator EInvoke(Unit unit)
    {
        throw new System.NotImplementedException();
    }
}
