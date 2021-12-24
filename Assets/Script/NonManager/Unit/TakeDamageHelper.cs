using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageHelper : MonoBehaviour
{
    public Unit owner;

    // tick
    private float tick = 0.1f;
    // ��ü ������
    private float totalDamage;
    // tick�� ���� ������
    private float tickDamage;
    // tick ����
    private int tickCountDown;
    // tick ������ ���� �ð�
    private float duraction;
    // �ð�
    private float time;
    // tick ������ Ȱ��ȭ
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
