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

    private void Start()
    {
        index = transform.GetSiblingIndex();
        if (!DeckManager.Select[index]) Destroy(gameObject);
        else
        {
            imgIllust.sprite = DeckManager.Select[index].Info.Icon;
            UIManager.Instance.FixSizeToRatio(imgIllust, 130);
        }
    }

    private void Update()
    {
        pressTime += Time.deltaTime;
        if (pressTime > needPressTime && down)
        {
            down = false;
            // 움직임이 정지함
            owner.deltaSpeed = owner.deltaSpeed == 1 ? 0 : 1;
            animator.SetBool("StopUiOpen", owner.deltaSpeed == 0);
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
        if (pressTime <= needPressTime)
        {
            animator.SetTrigger("SwitchSkill");
            // skill 사용
        }
    }

    public void ChangeSkillUI()
    {
        isSkillSprite = !isSkillSprite;

        if (isSkillSprite)
        {

        }
        else
        {

        }
    }
}
