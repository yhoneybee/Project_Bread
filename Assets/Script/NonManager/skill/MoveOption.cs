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

    [SerializeField, Header("몇초 동안 돌진을 하는지")]
    private float duraction = 5;
    [SerializeField, Header("얼마나 빠르게 돌진하는지")]
    private float speed = 1;
    [SerializeField, Header("유닛이 돌진중에 다른 유닛을 통과하는지")]
    private bool isTrigger;
    [SerializeField, Header("돌진중에 넉백이 있는지")]
    private bool hasKnockback;
    [SerializeField, Header("넉백 힘")]
    private Vector2 knockbackForce;
    [SerializeField, Header("돌진중에 데미지가 있는지")]
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
