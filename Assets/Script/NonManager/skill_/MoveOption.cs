using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "MoveOption", menuName = "Datas/Options/MoveOption")]
public class MoveOption : AOption
{
    public override Skill Context { get => context; set => context = value; }
    private Skill context;

    [SerializeField, Header("���� ���� ������ �ϴ���")]
    private float duraction = 5;
    [SerializeField, Header("�󸶳� ������ �����ϴ���")]
    private float speed = 1;
    [SerializeField, Header("������ �����߿� �ٸ� ������ ����ϴ���")]
    private bool isTrigger;
    [SerializeField, Header("�����߿� �˹��� �ִ���")]
    private bool hasKnockback;
    [SerializeField, Header("�����߿� �������� �ִ���")]
    private bool hasDamage;

    public override IEnumerator EInvoke(Unit unit)
    {
        var wait = new WaitForSeconds(0.001f);

        float time = 0;

        while (time >= duraction)
        {
            if (isTrigger)
                context.owner.transform.Translate(Vector3.right * speed * Time.deltaTime);
            else


            time += 0.001f;
        }

        yield return null;
    }
}
