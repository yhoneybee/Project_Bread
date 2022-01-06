using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "FormChangeOption", menuName = "Datas/Options/FormChangeOption")]
public class FormChangeOption : ABuffOption
{
    public override bool OnlyMe { get => false; set { return; } }
    public override bool IsInstant { get => isInstant; set => isInstant = value; }
    public override Skill Context { get => context; set => context = value; }
    private Skill context;

    [SerializeField, Header("AD MS AS HP MaxHP만 Buff효과로 적용됨")]
    private Stat buff;
    [SerializeField, Header("바뀌는 폼 스프라이트")]
    private Sprite sprChange;
    private Sprite sprBefore;
    [SerializeField, Header("지속 시간")]
    private float duraction;
    [SerializeField, Header("무적")]
    private bool hitAble;
    [SerializeField, Header("scale 변화")]
    private Vector2 toScale;
    [SerializeField, Header("즉발 여부")]
    private bool isInstant;

    public override IEnumerator EInvoke(Unit unit)
    {
        var wait = new WaitForSeconds(0.001f);

        var owner = context.owner;
        sprBefore = owner.SR.sprite;
        owner.SR.sprite = sprChange;

        owner.Stat *= buff;
        owner.Invincibility = hitAble;

        /*yield return new WaitForSeconds(duraction);*/

        float time = 0;

        while (time <= duraction)
        {
            if (Mathf.Abs(toScale.x - owner.transform.localScale.x) +
                Mathf.Abs(toScale.y - owner.transform.localScale.y) < 0.05f)
            {
                owner.transform.localScale = Vector3.Lerp(owner.transform.localScale, toScale, Time.deltaTime * 2);
            }

            time += 0.001f;
            yield return wait;
        }

        owner.transform.localScale = toScale;
        owner.Invincibility = false;

        owner.SR.sprite = sprBefore;

        float hp = owner.Stat.HP;

        owner.Stat = UnitManager.Instance.Units.Find(x => owner.Info.Name == x.Info.Name).Stat;
        owner.Stat.HP = hp;

        yield return null;
    }
}
