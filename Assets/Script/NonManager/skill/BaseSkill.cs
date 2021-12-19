using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : ScriptableObject
{
    public float CoolTime
    {
        get { return coolTime; }
        set 
        {
            if (value < 0)
            {
                coolTime = 0;
                return;
            }
            coolTime = value;
        }
    }
    public bool IsCoolTime
    {
        get
        {
            isCoolTime = CoolTime == 0;
            return isCoolTime;
        }
    }

    [SerializeField] private GameObject originSkill;
    [SerializeField] private float skillCoolTime;
    [SerializeField] private float coolTime;
    [SerializeField] private float duration;
    [SerializeField] private bool isCoolTime;

    public void Update()
    {
        CoolTime -= Time.deltaTime;
    }

    public void Cast()
    {
        CoolTime = skillCoolTime;
        // 여기서는 스킬을 직접 소환시키지 않고, 스킬을 소환을 일반화한 스킬 obj 소환 클래스의 go를 Instantiate함
        // 스킬 obj를 소환시키는 역할은 BaseSkill에 있지 않고 SkillSpawner(이름 미정)에게 위임함
        // SkillSpawner는 코루틴으로 그 역할을 세분화하는데,
        // 여기서 SkillSpawner안에 있는 코루틴을 실행시키는 함수를 스킬 종류에 맞게 인자를 제공하여 실행되도록 함
    }
}
