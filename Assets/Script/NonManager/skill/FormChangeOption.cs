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

    [SerializeField, Header("AD MS AS HP MaxHP�� Buffȿ���� �����")]
    private Stat buff;
    [SerializeField, Header("�ٲ�� �� ��������Ʈ")]
    private Sprite sprChange;
    private Sprite sprBefore;
    [SerializeField, Header("���� �ð�")]
    private float duraction;
    [SerializeField, Header("����")]
    private bool hitAble;
    [SerializeField, Header("scale ��ȭ")]
    private Vector2 toScale;
    [SerializeField, Header("��� ����")]
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
