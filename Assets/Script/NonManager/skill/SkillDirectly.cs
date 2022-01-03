using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDirectly : BaseSkill
{
    public float moveLenght;
    public float speed;
    public float betweenAttackDelayTime = 0.3f;
    private float moveTo;
    private float betweenAttackDelayDown;

    protected override void Start()
    {
        base.Start();
    }

    public override void Cast()
    {
        base.Cast();
        moveTo = owner.transform.position.x + moveLenght;
    }

    protected override void Update()
    {
        base.Update();
        if (Mathf.Abs(moveTo - owner.transform.position.x) >= 0.1f)
        {
            owner.transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (betweenAttackDelayDown > 0) betweenAttackDelayDown -= Time.deltaTime;
            else betweenAttackDelayDown = 0;
        }
        if (duractionDone)
        {
            ResetCount();
            moveTo = owner.transform.position.x;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var unit = collision.GetComponent<Unit>();
        if (countDown > 0 && betweenAttackDelayDown == 0)
        {
            countDown--;
            IngameManager.Instance.DamageText((int)totalDamage, owner.transform.position);
            unit.Stat.HP -= totalDamage;
        }
    }
}
