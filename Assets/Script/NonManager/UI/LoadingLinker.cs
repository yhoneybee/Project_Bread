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

    private float sin_value = 0;
    public float LoadingPersent
    {
        get { return loading_persent; }
        set
        {
            loading_persent = value;
            if (loading_persent >= 100)
            {
                loading_persent = 100;
                ButtonActions.Instance.ChangeScene("B - Main");
            }
            LoadingBar.value = loading_persent;
            Persent.text = $"{loading_persent}%";
        }
    }

    int loadSpeed = 1;

    private void Start()
    {
        InvokeRepeating(nameof(SpeedMulti), 0, 3f);
    }
    private void Update()
    {
        LoadingPersent += Time.deltaTime * loadSpeed;

        sin_value += Time.deltaTime * 50;

        Logo.position += new Vector3(0, 0.001f * Mathf.Sin(sin_value * Mathf.Deg2Rad));
    }

    public void SpeedMulti() => loadSpeed *= 5;
}
