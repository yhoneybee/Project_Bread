using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : Unit
{
    [SerializeField] Slider hp_slider;
    [SerializeField] Text hp_text;

    public override IEnumerator AttackedEffect(float damage)
    {
        base.AttackedEffect(damage);

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
            Destroy(gameObject);
        }

        hp_slider.value = Stat.HP / Stat.MaxHP;
        hp_text.text = Stat.HP.ToString();
    }
}
