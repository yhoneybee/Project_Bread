using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "FormChangeOption", menuName = "Datas/Options/FormChangeOption")]
public class FormChangeOption : ABuffOption
{
    public override bool OnlyMe { get => false; set { return; } }
    public override Skill Context { get => context; set => context = value; }
    private Skill context;

    [SerializeField, Header("바뀌는 폼 스프라이트")]
    private Sprite sprChange;
    [SerializeField, Header("지속 시간")]
    private float duraction;

    public override IEnumerator EInvoke(Unit unit)
    {
        var owner = context.owner;
        yield return new WaitForSeconds(duraction);
        yield return null;
    }
}
