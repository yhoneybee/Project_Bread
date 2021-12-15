using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSkill : BaseSkill
{
    public float durationTime;

    [SerializeField] private float radius;
    [SerializeField] private LineRenderer lr;
    [SerializeField] private CircleCollider2D colCircle2D;
    private float beforeRadius;

    private void Start()
    {
        beforeRadius = radius;

        lr.positionCount = 361;
        lr.useWorldSpace = false;

        CreateCircle();
    }

    public override void Update()
    {
        base.Update();
        if (beforeRadius != radius)
        {
            CreateCircle();
            colCircle2D.radius = radius;
        }

    }

    private void LateUpdate()
    {
        beforeRadius = radius;
    }

    private void CreateCircle()
    {
        return;
        float x;
        float y;
        float z = -1;
        float angle = 20f;
        for (int i = 0; i < 361; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            lr.SetPosition(i, new Vector3(x, y, z));
            angle += 1;
        }
    }

    public override void Cast(Unit target)
    {
        base.Cast(target);
        goSkill = Instantiate(this);
        Destroy(goSkill.gameObject, durationTime);
    }

    public override void Excute(Collider2D col2D)
    {
    }
}
