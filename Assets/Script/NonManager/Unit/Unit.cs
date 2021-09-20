using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Enums
public enum Rank
{
    COMMON,
    RARE,
    EPIC,
    LEGEND,
}

public enum AnimState
{
    IDLE,
    WALK,
    HIT,
    ATTACK,
}

public enum UnitType
{
    FRIEND,
    UNFRIEND,
}
#endregion

#region Structs
[Serializable]
public struct Info
{
    ///<summary>Unit Rank</summary>///
    public Rank Rank;
    ///<summary>Unit Name</summary>///
    public string Name;
    ///<summary>Unit UI Icon</summary>///
    public Sprite Icon;
    ///<summary>Unit Identity</summary>///
    public int Id;
    ///<summary>Unit Level</summary>///
    public int Level;
    ///<summary>Unit Cost</summary>///
    public int Cost;
    ///<summary>Unit Count</summary>///
    public int Count;
    ///<summary>Unit Description</summary>///
    public string Desc;
}

[Serializable]
public struct Stat
{
    ///<summary>AD : AttackDamage</summary>///
    public float AD;
    ///<summary>HP : HealthPoint</summary>///
    public float HP;
    ///<summary>MaxHP : MaxHealthPoint</summary>///
    public float MaxHP;
    ///<summary>MS : MoveSpeed</summary>///
    public float MS;
    ///<summary>AR : AttackRange</summary>///
    public float AR;
    ///<summary>AS : AttackSpeed</summary>///
    public float AS;
    ///<summary>LS : LifeSteal</summary>///
    public float LS;
    ///<summary>ºñ·Ê(ÆÛµ©) µ¥¹ÌÁö Á¤º¸</summary>///
    public Proportionality Proportionality;
}

[Serializable]
public struct Proportionality
{
    ///<summary>ÃÖ´ë Ã¼·Â ºñ·Ê</summary>///
    [Range(0.0f, 1.0f)]
    public float MaxHP;
    ///<summary>ÀÒÀº Ã¼·Â ºñ·Ê</summary>///
    [Range(0.0f, 1.0f)]
    public float LostHP;
    ///<summary>Àû ÃÖ´ë Ã¼·Â ºñ·Ê</summary>///
    [Range(0.0f, 1.0f)]
    public float EMaxHP;
    ///<summary>Àû ÀÒÀº Ã¼·Â ºñ·Ê</summary>///
    [Range(0.0f, 1.0f)]
    public float ELostHP;
    ///<summary>°ø°Ý·Â ºñ·Ê</summary>///
    [Range(0.0f, 1.0f)]
    public float AD;

    public float GetTotal(Unit taker, Unit taken)
    {
        float total = taker.Stat.AD * AD;
        total += (taker.Stat.MaxHP * MaxHP);
        total += (taker.Stat.MaxHP - taker.Stat.HP) * LostHP;
        total += (taken.Stat.MaxHP * EMaxHP);
        total += (taken.Stat.MaxHP - taken.Stat.HP) * ELostHP;

        return total;
    }
}

[Serializable]
public struct SpriteFrame
{
    public Sprite Sprite;
    public float Frame;
}

[Serializable]
public struct Anim
{
    public List<SpriteFrame> Idle;
    public List<SpriteFrame> Walk;
    public List<SpriteFrame> Hit;
    public List<SpriteFrame> Attack;
}
#endregion

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class Unit : MonoBehaviour
{
    private AnimState anim_state;
    public AnimState AnimState
    {
        get { return anim_state; }
        set
        {
            if (anim_state != value)
            {
                AnimIndex = 0;
                anim_state = value;
                OnAnimChanged();
            }
        }
    }

    public Info Info;
    public Stat Stat;
    public UnitType UnitType;
    public List<Item> Items = new List<Item>();
    public Anim Anim;

    SpriteRenderer SR;

    int AnimIndex = 0;
    float time = 0;

    Vector2 dir;

    protected virtual void Start()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        SR = GetComponent<SpriteRenderer>();
    }
    protected virtual void Update()
    {
        dir = UnitType == UnitType.FRIEND ? Vector2.right : Vector2.left;
        transform.Translate(dir * Stat.MS * Time.deltaTime);

        Animator();

        var hits = Physics2D.RaycastAll(transform.position, dir, 1 * Stat.AR, LayerMask.NameToLayer("Unit"));
        Debug.DrawRay(transform.position, dir * Stat.AR);

        foreach (var hit in hits)
        {
            if (hit.transform.gameObject != gameObject)
            {
                OnAttack(hit.transform.GetComponent<Unit>());
                break;
            }
        }
    }

    public void Animator()
    {
        time += Time.deltaTime;

        switch (AnimState)
        {
            case AnimState.IDLE:
                Animation(Anim.Idle);
                if (AnimIndex == Anim.Idle.Count)
                {
                    AnimIndex = 0;
                    OnEndFrameAnim();
                }
                break;
            case AnimState.WALK:
                Animation(Anim.Walk);
                if (AnimIndex == Anim.Walk.Count)
                {
                    AnimIndex = 0;
                    OnEndFrameAnim();
                }
                break;
            case AnimState.HIT:
                Animation(Anim.Hit);
                if (AnimIndex == Anim.Hit.Count)
                {
                    AnimState = AnimState.IDLE;
                    AnimIndex = 0;
                    OnEndFrameAnim();
                }
                break;
            case AnimState.ATTACK:
                Animation(Anim.Attack);
                if (AnimIndex == Anim.Attack.Count)
                {
                    AnimState = AnimState.IDLE;
                    AnimIndex = 0;
                    OnEndFrameAnim();
                }
                break;
        }
    }
    void Animation(List<SpriteFrame> SF)
    {
        if (SF.Count == 0) return;
        if (SR) SR.sprite = SF[AnimIndex].Sprite;
        if (time >= SF[AnimIndex].Frame) ++AnimIndex;
    }

    public virtual void OnAttack(Unit taken)
    {
        foreach (var item in Items) item.OnAttack(taken);
        taken.OnHit(this, Stat.AD);
    }
    public abstract void OnHit(Unit taker, float damage);
    public abstract void OnAnimChanged();
    public abstract void OnEndFrameAnim();
}
