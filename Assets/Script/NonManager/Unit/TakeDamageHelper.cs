using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageHelper : MonoBehaviour
{
    public Unit owner;

    // tick
    private float tick = 0.1f;
    // 전체 데미지
    private float totalDamage;
    // tick당 들어가는 데미지
    private float tickDamage;
    // tick 개수
    private int tickCountDown;
    // tick 데미지 지속 시간
    private float duraction;
    // 시간
    private float time;
    // tick 데미지 활성화
    private bool isContinueTick;

    private void Start()
    {
        owner = GetComponent<Unit>();
    }

    private void Update()
    {
        if (isContinueTick && time < duraction)
        {
            time += Time.deltaTime;
            if (time > tick * ((duraction / tick) - tickCountDown) && tickCountDown > 0)
            {
                owner.AnimState = AnimState.HIT;
                IngameManager.Instance.StartCoroutine(IngameManager.Instance.DamageText(((int)tickDamage), owner.transform.position));
                owner.Stat.HP -= tickDamage;
                tickCountDown--;
            }
        }
        else
        {
            time = 0;
            isContinueTick = false;
        }
    }

    public void StartTickDamage(float totalDamage, float duraction = 0.1f, float tick = 0.1f)
    {
        isContinueTick = true;
        this.duraction = duraction;
        this.totalDamage = totalDamage;
        this.tick = tick;
        tickCountDown = ((int)(duraction / tick));
        tickDamage = totalDamage / (tickCountDown);
        time = 0;
    }
}
