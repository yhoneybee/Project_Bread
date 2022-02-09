using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngameUnitBtnLinker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Unit owner;
    public Image imgCard;
    public Image imgIllust;
    public Image imgStop;
    public Text txtCool;
    public Animator animator;

    private float pressTime;
    private bool down;
    private bool isSkillSprite;
    private readonly float needPressTime = 0.5f;
    [SerializeField] private bool isSkillCast;

    private void Start()
    {
        if (!owner) Destroy(gameObject);
        else
        {
            imgIllust.sprite = owner.GetComponent<Skill>().sprSkill;
            UIManager.Instance.FixSizeToRatio(imgIllust, 130);
        }
    }

    private void Update()
    {
        //if (!owner || owner.Stat.HP < 0) Destroy(gameObject);
        imgIllust.gameObject.SetActive(owner && owner.Stat.HP > 0);
        imgCard.gameObject.SetActive(owner && owner.Stat.HP > 0);

        pressTime += Time.deltaTime;
        if (pressTime > needPressTime && down)
        {
            down = false;
            // �������� ������
            owner.deltaSpeed = owner.deltaSpeed == 1 ? 0 : 1;
            animator.SetBool("StopUiOpen", owner.deltaSpeed == 0);
        }
        if (owner && owner.skill && owner.skill.cool.CoolDone && isSkillCast)
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
            // skill ���
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
