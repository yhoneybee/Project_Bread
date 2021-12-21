using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillExcuter : MonoBehaviour
{
    public BaseSkillObj baseSkillObj;
    public Rigidbody2D rb2D;
    public CircleCollider2D circle2D;
    public BoxCollider2D box2D;

    private void OnTriggerEnter2D(Collider2D col2d)
    {
        var unit = col2d.GetComponent<Unit>();
        if (unit && unit.UnitType == UnitType.UNFRIEND) baseSkillObj.Excute(unit);
    }
    private void OnTriggerExit2D(Collider2D col2d)
    {
        var unit = col2d.GetComponent<Unit>();
        if (unit && unit.UnitType == UnitType.UNFRIEND) baseSkillObj.UnExcute(unit);
    }
}
