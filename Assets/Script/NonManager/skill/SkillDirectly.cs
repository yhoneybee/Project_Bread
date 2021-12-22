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

    public override void Cast()
    {
        base.Cast();
        betweenAttackDelayDown = betweenAttackDelayTime;
        moveTo = owner.transform.position.x + moveLenght;
        Invoke(nameof(ResetCount), duraction);
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
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var unit = collision.GetComponent<Unit>();
        if (countDown > 0)
        {
            countDown--;
            IngameManager.Instance.StartCoroutine(IngameManager.Instance.DamageText((int)totalDamage, owner.transform.position));
            unit.Stat.HP -= totalDamage;
        }
    }
}
