using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingLinker : MonoBehaviour
{
    public RectTransform Logo;
    public TextMeshProUGUI txtTouchToStart;

    [SerializeField] private float loading_persent;

    int loadSpeed = 1;
    Image logo_image;
    private void Start()
    {
        InvokeRepeating(nameof(SpeedMulti), 0, 3f);
        StartCoroutine(LogoImageFill());
        StartCoroutine(ETextTypeEffect());

        logo_image = Logo.GetComponent<Image>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ButtonActions.Instance.ChangeScene("B-Main");
        }
    }

    public void SpeedMulti() => loadSpeed *= 5;

    IEnumerator ETextTypeEffect()
    {
        string touchToStart = "터치하여 시작하세요!";
        var typing = new WaitForSeconds(0.1f);
        var wait = new WaitForSeconds(1);
        while (true)
        {
            for (int i = 0; i <= touchToStart.Length; i++)
            {
                txtTouchToStart.text = touchToStart.Substring(0, i);
                yield return typing;
            }
            yield return wait;
        }
    }

    IEnumerator LogoImageFill()
    {
        yield return new WaitForSeconds(0.5f);

        WaitForSeconds second = new WaitForSeconds(0.01f);

        while (1 - logo_image.fillAmount > 0.01f)
        {
            logo_image.fillAmount = Mathf.Lerp(logo_image.fillAmount, 1, 0.1f);
            yield return second;
        }
        logo_image.fillAmount = 1;

        StartCoroutine(LogoAnimation());
    }
    IEnumerator LogoAnimation()
    {
        WaitForSeconds second = new WaitForSeconds(0.01f);
        Vector2 target_scale;

        while (true)
        {
            target_scale = new Vector2(0.8f, 0.8f);
            while (Vector2.Distance(Logo.localScale, target_scale) > 0.01f)
            {
                Logo.localScale = Vector2.Lerp(Logo.localScale, target_scale, 0.01f);
                yield return second;
            }

            target_scale = new Vector2(1.2f, 1.2f);
            while (Vector2.Distance(Logo.localScale, target_scale) > 0.01f)
            {
                Logo.localScale = Vector2.Lerp(Logo.localScale, target_scale, 0.01f);
                yield return second;
            }

            yield return null;
        }
    }
}
