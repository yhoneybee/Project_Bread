using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Unit
{
    [SerializeField] Vector2 position;
    [SerializeField] Vector2 text_position;
    [SerializeField] Canvas main_canvas;
    [SerializeField] GameObject text_object;

    public override IEnumerator AttackedEffect()
    {
        transform.position = position;
        float random_x, random_y;

        GameObject textObject = Instantiate(text_object, text_position, Quaternion.identity, main_canvas.transform);
        for (int i = 0; i < 20; i++)
        {
            random_x = Random.Range(-0.1f, 0.1f);
            random_y = Random.Range(-0.05f, 0.05f);
            transform.Translate(new Vector2(random_x, random_y));
            yield return new WaitForSeconds(0.01f);
            transform.Translate(new Vector2(-random_x, -random_y));

            textObject.transform.Translate(Vector2.up / 10);
        }

        Destroy(textObject);
        base.AttackedEffect();
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
    }
}
