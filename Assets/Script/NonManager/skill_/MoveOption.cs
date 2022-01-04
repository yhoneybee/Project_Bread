using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "MoveOption", menuName = "Datas/Options/MoveOption")]
public class MoveOption : AOption
{
    public override Skill Context { get => context; set => context = value; }
    private Skill context;

    [SerializeField, Header("몇초 동안 돌진을 하는지")]
    private float duraction = 5;
    [SerializeField, Header("얼마나 빠르게 돌진하는지")]
    private float speed = 1;
    [SerializeField, Header("유닛이 돌진중에 다른 유닛을 통과하는지")]
    private bool isTrigger;
    [SerializeField, Header("돌진중에 넉백이 있는지")]
    private bool hasKnockback;
    [SerializeField, Header("돌진중에 데미지가 있는지")]
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
