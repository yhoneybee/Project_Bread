using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShot : BaseSkill
{
    public float betweenShotDelayTime = 0.3f;
    public float speed;
    private float betweenShotDelayDown;

    public override void Cast()
    {
        base.Cast();
    }

    protected override void Update()
    {
        base.Update();

        if (betweenShotDelayDown > 0) betweenShotDelayDown -= Time.deltaTime;
        else
        {
            if (countDown > 0)
            {
                countDown--;
                betweenShotDelayDown = betweenShotDelayTime;
                var obj = Instantiate(goSkill, owner.transform.position, Quaternion.identity);
                var goRight = obj.AddComponent<GoRight>();
                goRight.speed = speed;
                goRight.duraction = duraction;
            }
        }
    }
}
