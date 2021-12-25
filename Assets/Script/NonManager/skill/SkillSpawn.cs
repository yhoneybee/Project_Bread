using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSpawn : BaseSkill
{
    public Vector2 cell;
    public float randomAbsValue;
    public bool isNear;
    public bool isGuide;
    public float betweenSpawnDelayTime = 0.3f;
    public float speed;
    private float betweenSpawnDelayDown;
    private bool wasAttackAnimEnd;

    protected override void Start()
    {
        base.Start();
        owner.onAnimEndFrame += (animState) =>
        {
            if (animState == AnimState.ATTACK)
                wasAttackAnimEnd = true;
        };
    }

    public override void Cast()
    {
        base.Cast();
    }

    protected override void Update()
    {
        if (!wasAttackAnimEnd) return;

        base.Update();

        if (betweenSpawnDelayDown > 0) betweenSpawnDelayDown -= Time.deltaTime;
        else
        {
            if (countDown > 0)
            {
                countDown--;
                betweenSpawnDelayDown = betweenSpawnDelayTime;
                var obj = Instantiate(originSkill, owner.transform.position, Quaternion.identity);
                if (isNear)
                    obj.transform.position += new Vector3(Random.Range(-randomAbsValue, randomAbsValue), Random.Range(-randomAbsValue, randomAbsValue)) * (countDown + 1);
                else
                    obj.transform.position += new Vector3(cell.x, cell.y) * (countDown + 1);
                var goRight = obj.gameObject.AddComponent<GoRight>();
                goRight.speed = speed;
                goRight.duraction = duractionObj;
                goRight.isGuide = isGuide;
                var skillObj = obj.gameObject.AddComponent<SkillObj>();
                var anim = skillObj.GetComponent<Animator>();
                anim.runtimeAnimatorController = controller;
                anim.Play("Cast");
                skillObj.onEnter += (unit) => 
                {
                    unit.GetComponent<TakeDamageHelper>().StartTickDamage(totalDamage);
                };
            }
        }
    }
}
