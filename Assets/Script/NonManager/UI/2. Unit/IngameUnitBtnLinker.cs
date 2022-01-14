using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngameUnitBtnLinker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Unit owner;
    public Image imgIllust;
    public Text txtCool;
    public Animator animator;

    private float pressTime;
    private int index;
    private bool down;
    private bool isSkillSprite;
    private readonly float needPressTime = 0.5f;
    [SerializeField] private bool isSkillCast;

    private void Start()
    {
        index = transform.GetSiblingIndex();
        if (!DeckManager.Select[index]) Destroy(gameObject);
        else
        {
            imgIllust.sprite = owner.GetComponent<Skill>().sprSkill;
            UIManager.Instance.FixSizeToRatio(imgIllust, 130);
        }
    }

    private void Update()
    {
        if (!owner || owner.Stat.HP < 0) Destroy(gameObject);
        pressTime += Time.deltaTime;
        if (pressTime > needPressTime && down)
        {
            down = false;
            // 움직임이 정지함
            owner.deltaSpeed = owner.deltaSpeed == 1 ? 0 : 1;
            animator.SetBool("StopUiOpen", owner.deltaSpeed == 0);
        }
        if (owner.skill && owner.skill.cool.CoolDone && isSkillCast)
        {
            isSkillCast = false;
            animator.SetTrigger("SwitchIcon");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        down = true;
        pressTime = 0;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        down = false;
        if (pressTime <= needPressTime && owner.skill.cool.CoolDone)
        {
            // skill 사용
            isSkillCast = true;
            animator.SetTrigger("SwitchSkill");
            owner.skill.Cast();
        }
    }

    public void ChangeSkillUI()
    {
        isSkillSprite = !isSkillSprite;

        if (isSkillSprite)
        {
            imgIllust.sprite = owner.Info.Icon;
        }
        else
        {
            imgIllust.sprite = owner.skill.sprSkill;
        }
        UIManager.Instance.FixSizeToRatio(imgIllust, imgIllust.transform.parent.GetComponent<RectTransform>().sizeDelta.x - 20);
    }
}
