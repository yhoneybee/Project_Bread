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
        // ���⼭�� ��ų�� ���� ��ȯ��Ű�� �ʰ�, ��ų�� ��ȯ�� �Ϲ�ȭ�� ��ų obj ��ȯ Ŭ������ go�� Instantiate��
        // ��ų obj�� ��ȯ��Ű�� ������ BaseSkill�� ���� �ʰ� SkillSpawner(�̸� ����)���� ������
        // SkillSpawner�� �ڷ�ƾ���� �� ������ ����ȭ�ϴµ�,
        // ���⼭ SkillSpawner�ȿ� �ִ� �ڷ�ƾ�� �����Ű�� �Լ��� ��ų ������ �°� ���ڸ� �����Ͽ� ����ǵ��� ��
    }
}
