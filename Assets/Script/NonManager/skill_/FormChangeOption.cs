using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "FormChangeOption", menuName = "Datas/Options/FormChangeOption")]
public class FormChangeOption : ABuffOption
{
    public override bool OnlyMe { get => false; set { return; } }
    public override Skill Context { get => context; set => context = value; }
    private Skill context;

    [SerializeField, Header("�ٲ�� �� ��������Ʈ")]
    private Sprite sprChange;
    [SerializeField, Header("���� �ð�")]
    private float duraction;

    public override IEnumerator EInvoke(Unit unit)
    {
        var owner = context.owner;
        yield return new WaitForSeconds(duraction);
        yield return null;
    }
}
