using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseSkill", menuName = "Datas/BaseSkill", order = 0)]
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

    [HideInInspector] public Unit owner;
    public Sprite skillIcon;
    public RuntimeAnimatorController controller;

    [SerializeField] private BaseSkillObj skillObj;
    [SerializeField] private SkillExcuter originSkill;
    [SerializeField] private float skillCoolTime;
    [SerializeField] private float coolTime;
    [SerializeField] private bool isCoolTime;
    public void Update()
    {
        CoolTime -= Time.deltaTime;
    }

    public void Cast()
    {
        CoolTime = skillCoolTime;

        var obj = Instantiate(originSkill, owner.transform.position, Quaternion.identity);
        obj.GetComponent<Animator>().runtimeAnimatorController = controller;
        obj.baseSkillObj = skillObj;
        // ���⼭�� ��ų�� ���� ��ȯ��Ű�� �ʰ�, ��ų�� ��ȯ�� �Ϲ�ȭ�� ��ų obj ��ȯ Ŭ������ go�� Instantiate��
        // ��ų obj�� ��ȯ��Ű�� ������ BaseSkill�� ���� �ʰ� SkillSpawner(�̸� ����)���� ������
        // SkillSpawner�� �ڷ�ƾ���� �� ������ ����ȭ�ϴµ�,
        // ���⼭ SkillSpawner�ȿ� �ִ� �ڷ�ƾ�� �����Ű�� �Լ��� ��ų ������ �°� ���ڸ� �����Ͽ� ����ǵ��� ��
    }
}
