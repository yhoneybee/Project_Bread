using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSpawn : BaseSkill
{
    public Vector2 cell;
    public float randomAbsValue;
    public bool isNear;
    public bool isShot;
    public bool isGuide;
    public float betweenSpawnDelayTime = 0.3f;
    public float speed;
    private float betweenSpawnDelayDown;

    public override void Cast()
    {
        base.Cast();
    }

    protected override void Update()
    {
        base.Update();

        if (betweenSpawnDelayDown > 0) betweenSpawnDelayDown -= Time.deltaTime;
        else
        {
            if (countDown > 0)
            {
                countDown--;
                betweenSpawnDelayDown = betweenSpawnDelayTime;
                GameObject obj = Instantiate(originSkill, owner.transform.position, Quaternion.identity);
                if (isNear)
                    obj.transform.position += new Vector3(Random.Range(-randomAbsValue, randomAbsValue), Random.Range(-randomAbsValue, randomAbsValue)) * (countDown + 1);
                else
                    obj.transform.position += new Vector3(cell.x, cell.y) * (countDown + 1);
                if (isShot)
                {
                    var goRight = obj.AddComponent<GoRight>();
                    goRight.speed = speed;
                    goRight.duraction = duraction;
                    goRight.isGuide = isGuide;
                }
            }
        }
    }
}
