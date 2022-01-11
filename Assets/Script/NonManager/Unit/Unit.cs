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
    SKILL,
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
public struct SkillInfo
{
    public string name;
    public float duraction;
    public float duractionObj;
    public float tick;
}
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

    public static Stat operator *(Stat op1, Stat op2)
    {
        Stat result = op1;

        result.AD *= op2.AD == 0 ? 1 : op2.AD;
        result.MS *= op2.MS == 0 ? 1 : op2.MS;
        result.AS *= op2.AS == 0 ? 1 : op2.AS;
        result.HP *= op2.HP == 0 ? 1 : op2.HP;
        result.MaxHP *= op2.MaxHP == 0 ? 1 : op2.MaxHP;

        return result;
    }
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
                AnimState.SKILL => Anim.Skill,
                AnimState.DIE => Anim.Die,
                _ => null,
            };
            if (anim_state != value && (value != AnimState.WALK || AnimIndex >= anim.Count - 1) && anim_state != AnimState.DIE)
            {
                AnimIndex = 0;
                //print($"{Info.Name} anim was {anim_state} change to {value}");
                anim_state = value;
            }
        }
    }

    public Info Info;
    public Stat Stat;
    public UnitType UnitType;
    public List<Item> Items = new List<Item>();
    public Anim Anim;
    public Skill skill;
    public bool isTower;
    public event Action<AnimState> onAnimEndFrame;

    public SpriteRenderer SR { get; private set; }
    [HideInInspector] public Rigidbody2D rigid;
    BoxCollider2D coll;

    public int Need => (Info.Level + 9) * Info.Level;

    public bool WalkAble => is_walk_able;
    public bool AttakAble => is_walk_able;

    public bool Invincibility = false;

    public float deltaSpeed = 1;

    public bool isSkillObj;

    int AnimIndex = 0;
    float time = 0;
    bool is_walk_able = true;
    bool is_attack_able = true;
    [SerializeField] bool isDie = false;

    Vector2 dir;

    ParticleSystem particle_prefab;
    ParticleSystem walk_particle;

    protected virtual void Start()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation/* | RigidbodyConstraints2D.FreezePositionY*/;

        skill = GetComponent<Skill>();
        SR = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

        dir = UnitType == UnitType.FRIEND ? Vector2.right : Vector2.left;

        SR.sprite = Info.Icon;

        if (Anim == null) Anim = GetComponent<Anim>();

        isDie = false;

        if (isSkillObj) return;
        particle_prefab = Resources.Load<ParticleSystem>("Particle/Walk Particle");
        walk_particle = Instantiate(particle_prefab);
    }
    public void Init()
    {
        anim_state = AnimState.WALK;
        isDie = false;
        Stat.HP = Stat.MaxHP;
    }
    protected virtual void Update()
    {
        Animator();

        if (isDie) return;

        if (is_walk_able && attack_animation == null)
        {
            Moving();
        }
        AnimState = AnimState.WALK;

        if (Stat.HP > Stat.MaxHP) Stat.HP = Stat.MaxHP;

        CheckCollision();

        if (Stat.HP <= 0 && !isDie)
        {
            AnimState = AnimState.DIE;
            isDie = true;
        }
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
            case AnimState.SKILL:
                Animation(Anim.Skill);
                break;
            case AnimState.DIE:
                Animation(Anim.Die);
                if (AnimIndex == Anim.Die.Count - 1 || Anim.Die.Count == 0)
                {
                    if (!isTower)
                        UnitManager.Instance.ReturnUnit(this, null);
                    if (UnitType == UnitType.UNFRIEND)
                        IngameManager.Instance.ingameEnemies.Remove(this);
                }
                break;
        }
    }
    void Animation(List<Sprite> SF)
    {
        if (SR && SF.Count > 0) SR.sprite = SF[AnimIndex];
        if (time >= 0.1f)
        {
            ++AnimIndex;
            time = 0;
        }
        if (AnimIndex >= SF.Count)
        {
            AnimIndex = 0;
            onAnimEndFrame?.Invoke(AnimState);
        }
    }

    float sin_value = 0;
    public virtual void Moving()
    {
        if (isSkillObj) return;

        sin_value += Time.deltaTime * 1000;

        transform.Translate(dir.x *
            new Vector2(1 * Stat.MS, 5 * Mathf.Sin(sin_value * Mathf.Deg2Rad)) * Time.deltaTime * deltaSpeed);

        // Particle
        if (sin_value % 360 >= 170f && sin_value % 360 <= 180f)
        {
            walk_particle.transform.position = new Vector2(transform.position.x, -5);
            walk_particle.Play();
        }
    }

    void CheckCollision()
    {
        var hits = Physics2D.RaycastAll(transform.position, dir, 1 * Stat.AR + coll.size.x / 2, 1 << LayerMask.NameToLayer("Unit"));
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

                    if (is_attack_able && gameObject.activeSelf)
                        StartCoroutine(ASDelay(unit));

                    if (unit != null && unit.UnitType == UnitType.UNFRIEND && unit.gameObject.layer == 6)
                    {
                        if (skill != null && skill.move != null && !skill.cool.CoolDone)
                        {
                            if (skill.move.hasDamage)
                            {
                                if (skill.damage != null) skill.damage.EInvoke(unit);
                            }
                            if (skill.move.hasKnockback)
                            {
                                unit.rigid.AddForce(skill.move.knockbackForce);
                            }
                        }
                    }

                    break;
                }
                else
                    is_walk_able = true;
            }
        }
        else
        {
            is_walk_able = true;
        }
    }

    Coroutine attack_animation;
    Vector2 anim_start_pos;
    IEnumerator ASDelay(Unit unit)
    {
        is_attack_able = false;

        if (attack_animation != null)
        {
            StopCoroutine(attack_animation);
            transform.localPosition = anim_start_pos;
        }
        attack_animation = StartCoroutine(AttackAnimation(unit));

        yield return null;
    }
    IEnumerator AttackAnimation(Unit unit)
    {
        if (skill != null && !skill.cool.CoolDone) yield break;

        WaitForSeconds second = new WaitForSeconds(0.01f);

        Quaternion target_q = Quaternion.Euler(0, 0, -30.0f);
        anim_start_pos = transform.localPosition;
        Vector2 target_pos = anim_start_pos + dir * Stat.AR;
        float rot_lerp_f = 0.6f;
        float pos_lerp_f = 0.5f;

        for (int i = 0; i < 2; i++)
        {
            while (true)
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation,
                    target_q, rot_lerp_f);

                transform.localPosition = Vector2.Lerp(transform.localPosition, target_pos, pos_lerp_f);

                if (Mathf.Abs(transform.localRotation.z - target_q.z) <= 0.001f &&
                    Mathf.Abs(target_pos.x - transform.localPosition.x) <= 0.01f)
                    break;

                yield return second;
            }
            transform.localRotation = target_q;
            transform.localPosition = target_pos;

            if (i == 0)
            {
                target_q = Quaternion.Euler(0, 0, 0);
                target_pos = anim_start_pos;
                rot_lerp_f = 0.6f;
                pos_lerp_f = 0.3f;

                OnAttack(unit);
            }
        }

        attack_animation = null;

        yield return new WaitForSeconds(1.0f / Stat.AS - 0.1f);
        is_attack_able = true;
    }

    public virtual void OnAttack(Unit taken)
    {
        foreach (var item in Items) item.OnAttack(taken);

        taken.OnHit(this, Stat.AD + Stat.Proportionality.GetTotalDamage(this, taken));
        taken.AttackedEffect(Stat.AD);
        taken.AnimState = AnimState.HIT;

        if (IngameManager.Instance)
            IngameManager.Instance.DamageText(((int)Stat.AD), taken.transform.position);
    }
    public virtual void OnHit(Unit taker, float damage)
    {
        foreach (var item in Items) if (!Invincibility) item.OnHit(taker, ref damage);

        if (!Invincibility) Stat.HP -= damage;

        if (skill != null && skill.spawn != null && skill.spawn.isOnHit) skill.spawn.EInvoke(null);
    }

    Coroutine attacked_effect;
    public void AttackedEffect(float damage)
    {
        if (!gameObject.activeSelf) return;

        if (attacked_effect != null) StopCoroutine(attacked_effect);
        attacked_effect = StartCoroutine(_AttackedEffect(damage));
    }
    protected virtual IEnumerator _AttackedEffect(float damage)
    {
        if (!SR) yield break;

        WaitForSeconds second = new WaitForSeconds(0.01f);

        SR.color = Color.red;

        while (true)
        {
            yield return second;

            if (!SR) yield break;

            SR.color = Color.Lerp(SR.color, Color.white, Time.deltaTime * 3);

            if (SR.color.g >= 0.99f)
            {
                SR.color = Color.white;
                break;
            }
        }

        attacked_effect = null;
    }
}