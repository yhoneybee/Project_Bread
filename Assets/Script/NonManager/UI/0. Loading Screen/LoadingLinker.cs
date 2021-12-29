using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingLinker : MonoBehaviour
{
    public RectTransform Logo;
    public Slider LoadingBar;
    public TextMeshProUGUI Persent;

    [SerializeField] private float loading_persent;

    public float LoadingPersent
    {
        get { return loading_persent; }
        set
        {
            loading_persent = value;
            if (loading_persent >= 100)
            {
                LoadingBar.value = loading_persent;
                Persent.text = $"{(int)loading_persent}%";

                loading_persent = -9999999;
                ButtonActions.Instance.ChangeScene("B-Main");
            }
            else if (loading_persent > 0)
            {
                LoadingBar.value = loading_persent;
                Persent.text = $"{(int)loading_persent}%";
            }
        }
    }

    int loadSpeed = 1;
    Image logo_image;
    private void Start()
    {
        InvokeRepeating(nameof(SpeedMulti), 0, 3f);
        StartCoroutine(LogoImageFill());

        logo_image = Logo.GetComponent<Image>();
    }
    private void Update()
    {
        LoadingPersent += Time.deltaTime * loadSpeed;
    }

    public void SpeedMulti() => loadSpeed *= 5;

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
