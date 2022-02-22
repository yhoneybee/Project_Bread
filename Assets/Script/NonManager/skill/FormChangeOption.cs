using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "FormChangeOption", menuName = "Datas/Options/FormChangeOption")]
public class FormChangeOption : ABuffOption
{
    [SerializeField, Header("바뀌는 폼 스프라이트")]
    private Sprite sprChange;
    private Sprite sprBefore;
    [SerializeField, Header("무적")]
    private bool hitAble;
    [SerializeField, Header("scale 변화")]
    private Vector2 toScale;

    public override IEnumerator EInvoke(Unit unit)
    {
        var wait = new WaitForSeconds(0.01f);

        var owner = context.owner;

        while (owner.AnimState == AnimState.SKILL)
        {
            yield return wait;
        }
        owner.stopAnim = true;

        if (toScale == Vector2.zero) toScale = owner.transform.localScale;

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
                owner.transform.localScale = Vector3.Lerp(owner.transform.localScale, toScale, Time.deltaTime);
            }

            time += 0.01f;
            yield return wait;
        }

        owner.stopAnim = false;
        owner.transform.localScale = toScale;
        owner.Invincibility = false;

        owner.SR.sprite = sprBefore;

        float hp = owner.Stat.HP;

        owner.Stat = UnitManager.Instance.Units.Find(x => owner.Info.Name == x.Info.Name).Stat;
        owner.Stat.HP = hp;

        yield return null;
    }
}
