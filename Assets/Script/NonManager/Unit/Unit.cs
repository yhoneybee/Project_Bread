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
    //IDLE,
    WALK,
    HIT,
    ATTACK,
    DIE,
}

public enum UnitType
{
    FRIEND,
    UNFRIEND,
}

public enum AttackedEffectType
{
    Tower,
    Unit
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
    /// <summary> Image Devide Value </summary>///
    public float DValue;
    /// <summary> this Unit was get? </summary>///
    public bool Gotten => Count > 0 || Level > 1;
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
    ///<summary>���(�۵�) ������ ����</summary>///
    public Proportionality Proportionality;
}

[Serializable]
public struct Proportionality
{
    ///<summary>�ִ� ü�� ���</summary>///
    [Range(0.0f, 1.0f)]
    public float MaxHP;
    ///<summary>���� ü�� ���</summary>///
    [Range(0.0f, 1.0f)]
    public float LostHP;
    ///<summary>�� �ִ� ü�� ���</summary>///
    [Range(0.0f, 1.0f)]
    public float EMaxHP;
    ///<summary>�� ���� ü�� ���</summary>///
    [Range(0.0f, 1.0f)]
    public float ELostHP;
    ///<summary>���ݷ� ���</summary>///
    [Range(0.0f, 1.0f)]
    public float AD;

    public (float ad, float max_hp, float lost_hp, float e_max_hp, float e_lost_hp) GetDamages(Unit taker, Unit taken)
    {
        return (taker.Stat.AD * AD, taker.Stat.MaxHP * MaxHP, (taker.Stat.MaxHP - taker.Stat.HP) * LostHP, taken.Stat.MaxHP * EMaxHP, (taken.Stat.MaxHP - taken.Stat.HP) * ELostHP);
    }
    public float GetTotalDamage(Unit taker, Unit taken)
    {
        float sum = 0;
        sum += GetDamages(taker, taken).ad;
        sum += GetDamages(taker, taken).max_hp;
        sum += GetDamages(taker, taken).lost_hp;
        sum += GetDamages(taker, taken).e_max_hp;
        sum += GetDamages(taker, taken).e_lost_hp;
        return sum;
    }
}

//[Serializable]
//public struct Sprite
//{
//    public Sprite Sprite;
//    public float Frame;
//}
#endregion

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[Serializable]
public abstract class Unit : MonoBehaviour
{
    private AnimState anim_state;
    public AnimState AnimState
    {
        get { return anim_state; }
        set
        {
            if (!Anim) return;
            List<Sprite> anim = anim_state switch
            {
                AnimState.WALK => Anim.Walk,
                AnimState.HIT => Anim.Hit,
                AnimState.ATTACK => Anim.Attack,
                AnimState.DIE => Anim.Die,
                _ => null,
            };
            if (anim_state != value && (value != AnimState.WALK || AnimIndex >= anim.Count - 1) && anim_state != AnimState.DIE)
            {
                AnimIndex = 0;
                print($"{Info.Name} anim was {anim_state} change to {value}");
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
    public BaseSkill baseSkill;

    SpriteRenderer SR;
    Rigidbody2D rigid;
    BoxCollider2D coll;

    public int Need => (Info.Level + 9) * Info.Level;

    public bool WalkAble => is_walk_able;
    public bool AttakAble => is_walk_able;

    public bool Invincibility = false;

    public float deltaSpeed = 1;

    int AnimIndex = 0;
    float time = 0;
    bool is_walk_able = true;
    bool is_attack_able = true;
    bool isDie = false;

    Vector2 dir;

    protected virtual void Start()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation/* | RigidbodyConstraints2D.FreezePositionY*/;

        SR = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

        dir = UnitType == UnitType.FRIEND ? Vector2.right : Vector2.left;

        SR.sprite = Info.Icon;

        if (Anim == null) Anim = GetComponent<Anim>();

        isDie = false;
    }
    protected virtual void Update()
    {
        Animator();

        if (isDie) return;

        if (is_walk_able)
        {
            Moving();
        }
        AnimState = AnimState.WALK;

        if (Stat.HP > Stat.MaxHP) Stat.HP = Stat.MaxHP;

        var hits = Physics2D.RaycastAll(transform.position, dir, 1 * Stat.AR + coll.size.x / 2, 1 << LayerMask.NameToLayer("Unit")); ;
        Debug.DrawRay(transform.position, dir * (coll.size.x / 2) + dir * Stat.AR, Color.yellow);

        if (hits.Length > 1)
        {
            Unit unit;
            foreach (var hit in hits)
            {
                unit = hit.transform.GetComponent<Unit>();
                if (unit.UnitType != UnitType)
                {
                    is_walk_able = false;
                    if (is_attack_able)
                    {
                        OnAttack(unit);
                        AnimState = AnimState.ATTACK;
                        IngameManager.Instance.DamageText(((int)Stat.AD), unit.transform.position);
                        //ingame.StartCoroutine(ingame.DamageTextAnimation(unit.transform.position, Stat.AD));
                        unit.StartCoroutine(unit.AttackedEffect(Stat.AD));
                        unit.AnimState = AnimState.HIT;
                        if (gameObject.activeSelf) StartCoroutine(ASDelay());
                    }
                    break;
                }
                else
                    is_walk_able = true;
            }
        }
        else is_walk_able = true;

        if (Stat.HP <= 0 && !isDie)
        {
            SR.color = Color.white;
            AnimState = AnimState.DIE;
            isDie = true;
        }
    }

    public void Init()
    {
        Stat.HP = Stat.MaxHP;
    }
    public void Animator()
    {
        if (!Anim) return;
        time += Time.deltaTime;

        switch (AnimState)
        {
            case AnimState.WALK:
                Animation(Anim.Walk);
                break;
            case AnimState.HIT:
                Animation(Anim.Hit);
                break;
            case AnimState.ATTACK:
                Animation(Anim.Attack);
                break;
            case AnimState.DIE:
                Animation(Anim.Die);
                if (AnimIndex == Anim.Die.Count - 1 || Anim.Die.Count == 0)
                {
                    UnitManager.Instance.ReturnUnit(this, null);
                }
                break;
        }
    }
    void Animation(List<Sprite> SF)
    {
        if (SF.Count == 0) return;
        if (SR) SR.sprite = SF[AnimIndex];
        if (time >= 0.1f)
        {
            ++AnimIndex;
            time = 0;
        }
        if (AnimIndex == SF.Count)
        {
            AnimIndex = 0;
            OnEndFrameAnim();
        }
    }

    IEnumerator ASDelay()
    {
        is_attack_able = false;
        yield return new WaitForSeconds(1 / Stat.AS);
        is_attack_able = true;
        yield return null;
    }

    float sin_value = 0;
    public virtual void Moving()
    {
        sin_value += 5;

        transform.Translate(dir.x *
            new Vector2(1 * Stat.MS, 2 * Mathf.Sin(sin_value * Mathf.Deg2Rad)) * Time.deltaTime * deltaSpeed);
    }

    public virtual void OnAttack(Unit taken)
    {
        if (is_attack_able)
        {
            foreach (var item in Items) item.OnAttack(taken);
            taken.OnHit(this, Stat.AD + Stat.Proportionality.GetTotalDamage(this, taken));
        }
    }
    public virtual void OnHit(Unit taker, float damage)
    {
        foreach (var item in Items) if (!Invincibility) item.OnHit(taker, ref damage);

        if (!Invincibility) Stat.HP -= damage;
    }
    public virtual IEnumerator AttackedEffect(float damage)
    {
        if (!SR) yield break;

        SR.color = Color.red;

        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            if (!SR) yield break;

            SR.color = Color.Lerp(SR.color, Color.white, Time.deltaTime * 3);

            if (SR.color.g >= 0.99f)
            {
                SR.color = Color.white;
                break;
            }
        }
    }
    public abstract void OnAnimChanged();
    public abstract void OnEndFrameAnim();
}