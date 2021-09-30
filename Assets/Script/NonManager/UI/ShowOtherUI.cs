using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowOtherUI : MonoBehaviour
{
    [SerializeField] List<RectTransform> OtherUIs = new List<RectTransform>();
    [SerializeField] Button Button = null;

    [SerializeField] bool isAppeared = false;
    [SerializeField] float spacing = 15;
    [SerializeField] bool appear_right = true;

    Coroutine CAppear;
    Coroutine CHide;

    private void Start()
    {
        if (!Button) Button = GetComponent<Button>();
        Button.onClick.AddListener(() => { Switch(); });

        foreach (var ui in OtherUIs) ui.GetComponent<Button>().enabled = false;
    }

    public void Switch()
    {
        if (isAppeared)
        {
            Button.enabled = false;
            if (CHide != null) StopCoroutine(CHide);
            CHide = StartCoroutine(EHide());
            isAppeared = false;
        }
        else
        {
            Button.enabled = false;
            if (CAppear != null) StopCoroutine(CAppear);
            CAppear = StartCoroutine(EAppear());
            isAppeared = true;
        }
    }

    IEnumerator EAppear()
    {
        for (int i = 0; i < OtherUIs.Count; i++)
        {
            var otherUI = OtherUIs[i];
            otherUI.gameObject.SetActive(true);

            Vector2 move_to = (spacing + otherUI.sizeDelta.x) * (i + 1) * (appear_right ? Vector2.right : Vector2.left);

            if (appear_right)
            {
                while (otherUI.anchoredPosition.x < move_to.x)
                {
                    otherUI.anchoredPosition = Vector2.MoveTowards(otherUI.anchoredPosition, move_to, 25);
                    yield return new WaitForSeconds(0.0001f);
                }
            }
            else
            {
                while (otherUI.anchoredPosition.x > move_to.x)
                {
                    otherUI.anchoredPosition = Vector2.MoveTowards(otherUI.anchoredPosition, move_to, 25);
                    yield return new WaitForSeconds(0.0001f);
                }
            }

            otherUI.anchoredPosition = move_to;
        }

        foreach (var ui in OtherUIs) ui.GetComponent<Button>().enabled = true;

        Button.enabled = true;

        yield return null;
    }
    IEnumerator EHide()
    {
        for (int i = OtherUIs.Count - 1; i >= 0; i--)
        {
            var otherUI = OtherUIs[i];

            Vector2 move_to = otherUI.anchoredPosition + (spacing + otherUI.sizeDelta.x) * (i + 1) * (appear_right ? Vector2.left : Vector2.right);

            if (appear_right)
            {
                while (otherUI.anchoredPosition.x > move_to.x)
                {
                    otherUI.anchoredPosition = Vector2.MoveTowards(otherUI.anchoredPosition, move_to, 25);
                    yield return new WaitForSeconds(0.0001f);
                }
            }
            else
            {
                while (otherUI.anchoredPosition.x < move_to.x)
                {
                    otherUI.anchoredPosition = Vector2.MoveTowards(otherUI.anchoredPosition, move_to, 25);
                    yield return new WaitForSeconds(0.0001f);
                }
            }

            otherUI.anchoredPosition = move_to;

            otherUI.gameObject.SetActive(false);
        }

        foreach (var ui in OtherUIs) ui.GetComponent<Button>().enabled = false;

        Button.enabled = true;

        yield return null;
    }
}
