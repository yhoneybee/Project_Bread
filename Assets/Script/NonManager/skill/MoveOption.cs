using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "MoveOption", menuName = "Datas/Options/MoveOption")]
public class MoveOption : AMoveOption
{
    public override Skill Context { get => context; set => context = value; }
    private Skill context;

    public override bool HasKnockback { get => hasKnockback; set => hasKnockback = value; }
    public override Vector2 KnockbackForce { get => knockbackForce; set => knockbackForce = value; }
    public override bool HasDamage { get => hasDamage; set => hasDamage = value; }

    [SerializeField, Header("���� ���� ������ �ϴ���")]
    private float duraction = 5;
    [SerializeField, Header("�󸶳� ������ �����ϴ���")]
    private float speed = 1;
    [SerializeField, Header("������ �����߿� �ٸ� ������ ����ϴ���")]
    private bool isTrigger;
    [SerializeField, Header("�����߿� �˹��� �ִ���")]
    private bool hasKnockback;
    [SerializeField, Header("�˹� ��")]
    private Vector2 knockbackForce;
    [SerializeField, Header("�����߿� �������� �ִ���")]
    private bool hasDamage;

    public override IEnumerator EInvoke(Unit unit)
    {
        var wait = new WaitForSeconds(0.001f);

        float time = 0;

        float ms = context.owner.Stat.MS;

        while (time <= duraction)
        {
            if (isTrigger)
                context.owner.transform.Translate(Vector3.right * speed * Time.deltaTime);
            else
                context.owner.Stat.MS = speed;

            time += 0.001f;
            yield return wait;
        }

        context.owner.Stat.MS = ms;

        yield return null;
    }
}
