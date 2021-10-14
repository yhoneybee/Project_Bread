using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingLinker : MonoBehaviour
{
    public Slider LoadingBar;
    public TextMeshProUGUI Persent;

    [SerializeField] private float loading_persent;
    public float LoadingPersent
    {
        get { return loading_persent; }
        set
        {
            loading_persent = value;
            if (loading_persent >= 100) ButtonActions.Instance.ChangeScene("B - Main");
            LoadingBar.value = loading_persent;
            Persent.text = $"{loading_persent}%";
        }
    }

    int speed = 1;

    private void Start() => InvokeRepeating(nameof(SpeedMulti), 0, 3f);
    private void Update() => LoadingPersent += Time.deltaTime * speed;

    public void SpeedMulti() => speed *= 5;
}
