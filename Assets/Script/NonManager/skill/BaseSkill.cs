using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    public Unit owner;
    public Sprite skillIcon;
    public float coolTime;

    public virtual void Cast()
    {

    }
}
