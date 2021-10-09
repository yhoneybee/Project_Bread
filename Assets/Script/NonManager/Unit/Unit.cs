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
    /// <summary> Sprite Flip X </summary>///
    public bool FlipX;
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
    ///<summary>비례(퍼뎀) 데미지 정보</summary>///
    public Proportionality Proportionality;
}

[Serializable]
public struct Proportionality
{
    ///<summary>최대 체력 비례</summary>///
    [Range(0.0f, 1.0f)]
    public float MaxHP;
    ///<summary>잃은 체력 비례</summary>///
    [Range(0.0f, 1.0f)]
    public float LostHP;
    ///<summary>적 최대 체력 비례</summary>///
    [Range(0.0f, 1.0f)]
    public float EMaxHP;
    ///<summary>적 잃은 체력 비례</summary>///
    [Range(0.0f, 1.0f)]
    public float ELostHP;
    ///<summary>공격력 비례</summary>///
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

[Serializable]
public struct SpriteFrame
{
    public Sprite Sprite;
    public float Frame;
}
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

    public bool WalkAble => is_walk_able;
    public bool AttakAble => is_walk_able;

    int AnimIndex = 0;
    float time = 0;
    bool is_walk_able = true;
    bool is_attack_able = true;

    Vector2 dir;

    protected virtual void Start()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation/* | RigidbodyConstraints2D.FreezePositionY*/;
        SR = GetComponent<SpriteRenderer>();
        SR.sprite = Info.Icon;
        if (Anim == null) Anim = GetComponent<Anim>();

        foreach (var item in Items) item.Equip();
    }
    protected virtual void Update()
    {
        if (is_walk_able) Moving();

        Animator();

        var hits = Physics2D.RaycastAll(transform.position, dir, 1 * Stat.AR, 1 << LayerMask.NameToLayer("Unit"));
        Debug.DrawRay(transform.position, dir * Stat.AR, Color.yellow);

        if (Stat.HP <= 0)
        {
            SR.color = Color.white;
            UnitManager.Instance.ReturnUnit(this, null);
        }

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
                        if (gameObject.activeSelf) StartCoroutine(ASDelay());
                        unit.StartCoroutine(unit.AttackedEffect(Stat.AD));
                    }
                    break;
                }
                else
                    is_walk_able = true;
            }
        }
        else is_walk_able = true;
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

    IEnumerator ASDelay()
    {
        is_attack_able = false;
        yield return new WaitForSeconds(1 / Stat.AS);
        is_attack_able = true;
        yield return null;
    }

    float angle = 0;
    bool up;
    public virtual void Moving()
    {
        if (angle <= -90)
            up = true;
        else if (angle >= 90)
            up = false;

        // 상수값을 늘릴수록 위아래 폭이 넓어짐
        angle += Time.deltaTime * (up ? 1000 : -1000);

        dir = UnitType == UnitType.FRIEND ? Vector2.right : Vector2.left;

        // Sin 함수를 이용한 위 아래 움직임 구현
        transform.Translate(dir.x *
            Stat.MS *
            Time.deltaTime *
            new Vector2(1, Mathf.Sin(angle * Mathf.PI / 180)));

        SR.flipX = Info.FlipX;
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
        foreach (var item in Items) item.OnHit(taker, ref damage);

        Stat.HP -= damage;
    }
    public virtual IEnumerator AttackedEffect(float damage)
    {
        SR.color = Color.red;
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
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