using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : Unit
{
    [SerializeField] Canvas canvas;

    [SerializeField] GameObject text_object;
    [SerializeField] Slider hp_slider;
    [SerializeField] Text hp_text;
    List<GameObject> texts = new List<GameObject>();

    public override IEnumerator AttackedEffect(float damage)
    {
        StartCoroutine(TextAnimation(damage));

        //transform.position = position;
        float random_x, random_y;

        // �¾��� �� Ÿ�� ��鸮�� �κ�
        for (int i = 0; i < 20; i++)
        {
            random_x = Random.Range(-0.1f, 0.1f);
            random_y = Random.Range(-0.05f, 0.05f);
            transform.Translate(new Vector2(random_x, random_y));
            yield return new WaitForSeconds(0.01f);
            transform.Translate(new Vector2(-random_x, -random_y));
        }


        base.AttackedEffect(damage);
    }
    IEnumerator TextAnimation(float damage)
    {
        GameObject textObject = Instantiate(text_object, canvas.transform);
        textObject.transform.localPosition = new Vector2(Random.Range(-200, 200), 300);
        textObject.GetComponentInChildren<Text>().text = "- " + damage.ToString();

        texts.Add(textObject);

        while (true)
        {
            textObject.transform.Translate(Vector2.up / 7);
            yield return new WaitForSeconds(0.01f);

            if (textObject.transform.localPosition.y >= 1000)
            {
                Destroy(textObject);
                texts.Remove(textObject);
                break;
            }
        }
    }

    public override void OnAnimChanged()
    {
    }

    public override void OnEndFrameAnim()
    {
    }

    protected override void Start()
    {
    }

    protected override void Update()
    {
        if (Stat.HP <= 0)
        {
            foreach (var text in texts)
            {
                Destroy(text);
            }
            texts.Clear();

            Destroy(gameObject);
        }

        hp_slider.value = Stat.HP / Stat.MaxHP;
        hp_text.text = Stat.HP.ToString();
    }
}
